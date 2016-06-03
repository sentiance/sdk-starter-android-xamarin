//
// MyApplication.cs
//
using System;
using Android.App;
using Android.Content;
using Com.Sentiance.Sdk.Modules.Config;
using Android.Support.V4.App;
using Com.Sentiance.Sdk;
using Android.Support.V4.Content;

namespace Com.Sentiance.Sdkstarter.Droid
{
	[Application] 
	public class MyApplication : Application, IAuthenticationListener
	{

		public static string ACTION_SDK_AUTHENTICATION_SUCCESS = "ACTION_SDK_AUTHENTICATION_SUCCESS";
		const string APP_ID = "";
		const string APP_SECRET = "";

		public MyApplication (IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base (javaReference, transfer)
		{
		}
		

		public MyApplication ()
		{
		}

		#region public methods
		public override void OnCreate ()
		{
			base.OnCreate ();
			initializeSentianceSdk ();
		}

		public void OnAuthenticationFailed (string p0)
		{
			// Here you should wait, inform the user to ensure an internet connection and retry initializeSentianceSdk afterwards
			Console.WriteLine("SDKStarter - Error launching Sentiance SDK: "+p0);

			// Some SDK Starter specific help
			if(p0.Contains("Bad Request")) {
				Console.WriteLine("SDKStarter - You should create a developer account on https://audience.sentiance.com/developers and afterwards register a Sentiance application on https://audience.sentiance.com/apps\n" +
					"This will give you an application ID and secret which you can use to replace YOUR_APP_ID and YOUR_APP_SECRET in AppDelegate.m");
			}
		}

		public void OnAuthenticationSucceeded ()
		{
			// Called when the SDK was able to create a platform user
			Console.WriteLine("SDKStarter - Sentiance SDK started, version: "+Sdk.Sdk.GetInstance(this).Version);
			Console.WriteLine ("SDKStarter - Sentiance platform user id for this install: " + Sdk.Sdk.GetInstance (this).User ().Id);
			Console.WriteLine("SDKStarter - Authorization token that can be used to query the HTTP API: Bearer "+Sdk.Sdk.GetInstance(this).User().AccessToken);

			LocalBroadcastManager.GetInstance(ApplicationContext).SendBroadcast(new Intent(ACTION_SDK_AUTHENTICATION_SUCCESS));
		}

		#endregion

		#region private methods
		void initializeSentianceSdk() {
			// SDK configuration
			SdkConfig config = new SdkConfig(new SdkConfig.AppCredentials(
				APP_ID,
				APP_SECRET
			));

			// Let the SDK start the service foregrounded by showing a notification. This discourages Android from killing the process.
			Intent intent = new Intent(this, typeof(MainActivity)).SetFlags (ActivityFlags.ClearTop);
			PendingIntent pendingIntent = PendingIntent.GetActivity(this, 0, intent, 0);
			Notification notification = new NotificationCompat.Builder(this)
				.SetContentTitle(GetString(Resource.String.app_name) + " is running")
				.SetContentText("Touch to open.")
				.SetShowWhen(false)
				.SetSmallIcon(Resource.Mipmap.ic_launcher)
				.SetContentIntent(pendingIntent)
				.SetPriority(NotificationCompat.PriorityMin)
				.Build();

			config.EnableStartForegrounded (notification);


			// Register this instance as authentication listener
			Sdk.Sdk.GetInstance(this).SetAuthenticationListener(this);

			// Initialize and start the Sentiance SDK module
			// The first time an app installs on a device, the SDK requires internet to create a Sentiance platform userid
			Sdk.Sdk.GetInstance (this).Init (config);
		}
		#endregion
	}
}

