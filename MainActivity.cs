using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.Content;
using Android;
using Android.Content;
using System;
using Java.Lang;
using Android.Content.PM;
using System.Collections.Generic;
using Com.Sentiance.Sdk;

namespace Com.Sentiance.Sdkstarter.Droid
{
	[Activity (Label = "@string/app_name", MainLauncher = true, Icon = "@mipmap/ic_launcher")]
	public class MainActivity : AppCompatActivity
	{
		ListView statusList;
		BroadcastReceiver authenticationBroadcastReciever;
		IRunnable refreshStatusRunnable;
		Handler handler = new Handler();

		public MainActivity (IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base (javaReference, transfer)
		{
		}
		

		public MainActivity ()
		{
		}
		

		#region protected methods
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Permission.Granted) {
				// We need to ask the user to grant permission. We've offloaded that to a different activity for clarity.
				StartActivity(new Intent(this, typeof(PermissionActivity)));
			}

			SetContentView(Resource.Layout.activity_main);

			statusList = (ListView) FindViewById(Resource.Id.statusList);
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			if (authenticationBroadcastReciever == null) {
				authenticationBroadcastReciever = new LocalBroadcastReceiver (refreshStatus);
				refreshStatusRunnable = new LocalRunnable (refreshStatus, handler);
			}

			// Our MyApplication broadcasts when the SDK authentication was successful
			LocalBroadcastManager.GetInstance(ApplicationContext).RegisterReceiver(authenticationBroadcastReciever, new IntentFilter(MyApplication.ACTION_SDK_AUTHENTICATION_SUCCESS));

			// Periodically refresh the status UI
			handler.Post(refreshStatusRunnable);

			refreshStatus();
		}

		protected override void OnPause ()
		{
			base.OnPause ();

			LocalBroadcastManager.GetInstance(ApplicationContext).UnregisterReceiver(authenticationBroadcastReciever);
			handler.RemoveCallbacks (refreshStatus);
		}
		#endregion

		#region private methods
		void refreshStatus()
		{
			List<string> statusItems = new List<string>();
			statusItems.Add("SDK flavor: " + Sdk.Sdk.GetInstance(ApplicationContext).Flavor);
			statusItems.Add("SDK version: " +Sdk.Sdk.GetInstance(ApplicationContext).Version);

			// On Android, the user id is a re.ource url, using format https://api.sentiance.com/users/USER_ID, you can replace the part to obtain the short URL code:
			var userId = Sdk.Sdk.GetInstance(ApplicationContext).User ().Id;
			if (userId.IsPresent) {
				
				statusItems.Add("User ID: " + userId.Get ().ToString ().Replace ("https://api.sentiance.com/users/", ""));
			} else {
				statusItems.Add("User ID: N/A");
			}

			// You can use the status message to obtain more information
			StatusMessage statusMessage = Sdk.Sdk.GetInstance(ApplicationContext).StatusMessage;
			statusItems.Add("Mode: " + statusMessage.Mode.Name ());

			foreach(SdkIssue issue in statusMessage.Issues) {
				statusItems.Add("Issue: " + issue.Type.Name());
			}

			statusItems.Add("Wi-Fi data: " + statusMessage.WifiQuotaUsed + " / " + statusMessage.WifiQuotaLimit);
			statusItems.Add("Mobile data: " + statusMessage.MobileQuotaUsed + " / " + statusMessage.MobileQuotaLimit);
			statusItems.Add("Disk: " + statusMessage.DiskQuotaUsed + " / " + statusMessage.DiskQuotaLimit);

			DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0,DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds( statusMessage.WifiLastSeenTimestamp/1000 ).ToLocalTime();
			statusItems.Add("Wi-Fi last seen: " + dtDateTime.ToString ("yyyy-MM-dd HH:mm:ss"));

			statusList.Adapter =  new ArrayAdapter(this, Resource.Layout.list_item_status, Resource.Id.textView, statusItems);
		}
		#endregion

		#region internal class
		internal class LocalBroadcastReceiver : BroadcastReceiver
		{
			Action ReceiveAction;

			public LocalBroadcastReceiver (IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base (javaReference, transfer)
			{
			}

			public LocalBroadcastReceiver (Action receiveAction)
			{
				this.ReceiveAction = receiveAction;
			}

			public override void OnReceive (Context context, Intent intent)
			{
				ReceiveAction ();
			}
		}

		internal class LocalRunnable : Java.Lang.Object, IRunnable 
		{
			const long STATUS_REFRESH_INTERVAL_MILLIS = 5000;

			Action RunAction;
			Handler RunHandler;

			public LocalRunnable (IntPtr handle, Android.Runtime.JniHandleOwnership transfer) : base (handle, transfer)
			{
			}
			

			public LocalRunnable (Action runAction, Handler runHandler)
			{
				this.RunAction = runAction;
				this.RunHandler = runHandler;
			}

			public void Run ()
			{
				RunAction ();
				RunHandler.PostDelayed (this, STATUS_REFRESH_INTERVAL_MILLIS);
			}

			protected override void Dispose (bool disposing)
			{
				RunAction = null;
				base.Dispose (disposing);
			}
		}
		#endregion
	}
}


