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
using System.Collections.Generic;
using Android.Content;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace SDKStarter
{
    [Activity]
    public class PermissionActivity : AppCompatActivity
    {

        PermissionManager mPermissionManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mPermissionManager = new PermissionManager(this);

            List<Permission> permissions = mPermissionManager.GetNotGrantedPermissions();
            if (permissions.Count > 0)
            {
                RequestPermission(permissions[0], false);
            }
            else
            {
                StartMain();
            }
        }

        private void RequestPermission(Permission p, Boolean bypassRationale)
        {
            if (!bypassRationale && p.ShouldShowRationale(this))
            {
                ShowExplanation(p);
            }
            else if (!p.IsGranted(this))
            {
                ActivityCompat.RequestPermissions(this, p.GetManifestPermissions(), p.GetAskCode());
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            if (mPermissionManager.GetNotGrantedPermissions().Count == 0)
            {
                StartMain();
            }
            else
            {
                // Requesting new permissions from the same activity instance fails for some reason.
                Finish();
                StartActivity(new Intent(this, typeof(PermissionActivity)));
            }
        }

        private void ShowExplanation(Permission p)
        {
            new AlertDialog.Builder(this)
                    .SetTitle(p.GetDialogTitle())
                    .SetMessage(p.GetDialogMessage())
                    .SetPositiveButton("OK", (s,e) => { RequestPermission(p, true); })
                    .Show();
        }

        private void StartMain()
        {
            Finish();
            StartActivity(new Intent(this, typeof(MainActivity)));
        }
    }

}
