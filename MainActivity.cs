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
using Android.Text.Format;

namespace SDKStarter
{
	[Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@mipmap/ic_launcher")]
	public class MainActivity : AppCompatActivity
	{
		ListView statusList;
		BroadcastReceiver statusUpdateReceiver;

		public MainActivity(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}


		public MainActivity()
		{
		}


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Permission.Granted)
			{
				// We need to ask the user to grant permission. We've offloaded that to a different activity for clarity.
				StartActivity(new Intent(this, typeof(PermissionActivity)));
			}

			SetContentView(Resource.Layout.activity_main);

			statusList = (ListView)FindViewById(Resource.Id.statusList);
		}

		protected override void OnResume()
		{
			base.OnResume();

			if (statusUpdateReceiver == null)
			{
				statusUpdateReceiver = new LocalBroadcastReceiver(refreshStatus);

			}

			// Our MyApplication broadcasts when the SDK authentication was successful
			LocalBroadcastManager.GetInstance(ApplicationContext).RegisterReceiver(statusUpdateReceiver, new IntentFilter(MyApplication.ACTION_SENTIANCE_STATUS_UPDATE));

			refreshStatus();
		}

		protected override void OnPause()
		{
			base.OnPause();
			LocalBroadcastManager.GetInstance(ApplicationContext).UnregisterReceiver(statusUpdateReceiver);
		}

		private void refreshStatus()
		{
			List<string> statusItems = new List<string>();

			if (Sentiance.GetInstance(this).IsInitialized)
			{
				statusItems.Add("SDK version: " + Sentiance.GetInstance(ApplicationContext).Version);
				statusItems.Add("User ID: " + Sentiance.GetInstance(ApplicationContext).UserId);

				// You can use the status message to obtain more information
				SdkStatus sdkStatus = Sentiance.GetInstance(ApplicationContext).SdkStatus;

				statusItems.Add("Start status: " + sdkStatus.SdkStartStatus.Name());
				statusItems.Add("Can detect: " + sdkStatus.CanDetect);
				statusItems.Add("Remote enabled: " + sdkStatus.IsRemoteEnabled);
				statusItems.Add("Location perm granted: " + sdkStatus.IsLocationPermGranted);
				statusItems.Add("Location setting: " + sdkStatus.AndroidLocationSetting);
				
				statusItems.Add(formatQuota("Wi-Fi", sdkStatus.WifiQuotaStatus, Sentiance.GetInstance(this).WiFiQuotaUsage, Sentiance.GetInstance(this).WiFiQuotaLimit));
				statusItems.Add(formatQuota("Mobile data", sdkStatus.MobileQuotaStatus, Sentiance.GetInstance(this).MobileQuotaUsage, Sentiance.GetInstance(this).MobileQuotaLimit));
				statusItems.Add(formatQuota("Disk", sdkStatus.DiskQuotaStatus, Sentiance.GetInstance(this).DiskQuotaUsage, Sentiance.GetInstance(this).DiskQuotaLimit));

                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
                {
                    statusItems.Add("Battery optimization enabled: " + sdkStatus.IsBatteryOptimizationEnabled);
                }
                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    statusItems.Add("Battery saving enabled: " + sdkStatus.IsBatterySavingEnabled);
                }
                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                {
                    statusItems.Add("Background processing restricted: " + sdkStatus.IsBackgroundProcessingRestricted);
                }
			}
			else
			{
				statusItems.Add("SDK not initialized");
			}

			statusList.Adapter = new ArrayAdapter(this, Resource.Layout.list_item_status, Resource.Id.textView, statusItems);
		}

		private string formatQuota(string name, SdkStatus.QuotaStatus status, long bytesUsed, long bytesLimit)
		{
			return System.String.Format("{0} quota: {1} / {2} ({3})",
										name,
										Formatter.FormatShortFileSize(this, bytesUsed),
										Formatter.FormatShortFileSize(this, bytesLimit),
			                            status);
		}

		internal class LocalBroadcastReceiver : BroadcastReceiver
		{
			Action ReceiveAction;

			public LocalBroadcastReceiver(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer)
			{
			}

			public LocalBroadcastReceiver(Action receiveAction)
			{
				this.ReceiveAction = receiveAction;
			}

			public override void OnReceive(Context context, Intent intent)
			{
				ReceiveAction();
			}
		}
	}
}


