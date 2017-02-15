//
// PermissionActivity.cs
//
using System;
using Android.Support.V7.App;
using Android.OS;
using Android.Support.V4.App;
using Android;
using Android.App;
using Android.Content.PM;

namespace SDKStarter
{
	[Activity]
	public class PermissionActivity : AppCompatActivity, Android.Content.IDialogInterfaceOnClickListener
	{

		static int PERMISSION_REQUEST_CODE = 15442;

		public PermissionActivity()
		{
		}

		public PermissionActivity(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		#region public methods

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
			{
				showExplanation();
			}
			else
			{
				requestPermission();
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			if (requestCode == PERMISSION_REQUEST_CODE)
			{
				if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
				{
					// Permission granted, proceed
					proceed();
				}
				else if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
				{
					// Permission denied, but we should show an explanation
					showExplanation();
				}
				else
				{
					// Permission denied, no explanation necessary. We can only proceed, even though the SDK will not actually be able to start detections.
					proceed();
				}
			}
		}

		public void OnClick(Android.Content.IDialogInterface dialog, int which)
		{
			requestPermission();
		}
		#endregion

		#region private methods

		void showExplanation()
		{
			new Android.Support.V7.App.AlertDialog.Builder(this)
				.SetTitle("Location Permission")
				.SetMessage("The Sentiance SDK needs access to your location in order to build your profile.")
				.SetPositiveButton("OK", this)
				.Show();
		}

		void requestPermission()
		{
			ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessFineLocation }, PERMISSION_REQUEST_CODE);
		}

		void proceed()
		{
			Finish();
		}

		#endregion

	}
}

