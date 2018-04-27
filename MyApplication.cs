//
// MyApplication.cs
//
using System;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using Com.Sentiance.Sdk;

namespace SDKStarter
{
	[Application]
	public class MyApplication : Application, IOnInitCallback, IOnStartFinishedHandler, IOnSdkStatusUpdateHandler
	{

		public static string ACTION_SENTIANCE_STATUS_UPDATE = "ACTION_SENTIANCE_STATUS_UPDATE";

        const string APP_ID = "YOUR_APP_ID";
        const string APP_SECRET = "YOUR_APP_SECRET";

		const string TAG = "SDKStarter";

		public MyApplication(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}


		public MyApplication()
		{
		}

		public override void OnCreate()
		{
			base.OnCreate();
			initializeSentianceSdk();
		}

		private void initializeSentianceSdk()
		{   
			// Create the config.
            SdkConfig config = new SdkConfig.Builder(APP_ID, APP_SECRET, createNotification())
				.SetOnSdkStatusUpdateHandler(this)
				.Build();

			// Initialize the SDK.
			Sentiance.GetInstance(this).Init(config, this);
		}

        private Notification createNotification()
        {
            // PendingIntent that will start your application's MainActivity
            Intent intent = new Intent(this, typeof(MainActivity));
            PendingIntent pendingIntent = PendingIntent.GetActivity(this, 0, intent, 0);

            // On Oreo and above, you must create a notification channel
            String channelId = "trips";
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O) {
                NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
                NotificationChannel channel = new NotificationChannel(channelId, "Trips", NotificationImportance.Min);
                channel.SetShowBadge(false);
                notificationManager.CreateNotificationChannel(channel);
            }

            return new NotificationCompat.Builder(this, channelId)
                                     .SetContentTitle(GetString(Resource.String.app_name) + " is running")
                                     .SetContentText("Touch to open.")
                                     .SetContentIntent(pendingIntent)
                                     .SetShowWhen(false)
                                     .SetSmallIcon(Resource.Mipmap.ic_launcher)
                                     .SetPriority((int)NotificationPriority.Min)
                                     .Build();
        }

		public void OnInitSuccess()
		{
			printInitSuccessLogStatements();
			Sentiance.GetInstance(this).Start(this);
		}

		public void OnInitFailure(OnInitCallbackInitIssue issue)
		{
			Log.Info(TAG, "Could not initialize SDK: " + issue);
		}

		public void OnStartFinished(SdkStatus status)
		{
			Log.Info(TAG, "SDK start finished with status: " + status.SdkStartStatus);
		}

		public void OnSdkStatusUpdate(SdkStatus status)
		{
			Log.Info("SDKStarter", "SDK status updated: " + status.ToString());
			LocalBroadcastManager.GetInstance(this).SendBroadcast(new Intent(ACTION_SENTIANCE_STATUS_UPDATE));
		}

		private void printInitSuccessLogStatements()
		{
			Log.Info(TAG, "Sentiance SDK initialized, version: " + Sentiance.GetInstance(this).Version);
			Log.Info(TAG, "Sentiance platform user id for this install: " + Sentiance.GetInstance(this).UserId);

			Sentiance.GetInstance(this).GetUserAccessToken(new TokenCallback());
		}

		internal class TokenCallback : Java.Lang.Object, ITokenResultCallback
		{
			
			public void OnFailure()
			{
				Log.Error(TAG, "Couldn't get access token");
			}

			public void OnSuccess(Token token)
			{
				Log.Info(TAG, "Access token to query the HTTP API: Bearer " + token.TokenId);
			}
		}
	}
}

