using System;
using System.Collections.Generic;
using Android.App;

namespace SDKStarter
{
    public class PermissionManager
    {
        private static readonly int LOCATION_PERMISSION_REQUEST_CODE = 15442;
        private static readonly int ACTIVITY_PERMISSION_REQUEST_CODE = 15443;
        private static readonly string TITLE_LOCATION = "Location permission";
        private static readonly string MESSAGE_LOCATION = "The Sentiance SDK needs access to your location all the time in order to build your profile.";
        private static readonly string TITLE_ACTIVITY = "Motion activity permission";
        private static readonly string MESSAGE_ACTIVITY = "The Sentiance SDK needs access to your activity data in order to build your profile.";

    private Activity mActivity;

        public PermissionManager(Activity activity)
        {
            mActivity = activity;

            UpdateCanShowAgainPermissions();
        }

        public List<Permission> GetNotGrantedPermissions()
        {
            List<Permission> notGrantedPermissions = new List<Permission>();

            foreach (Permission p in GetAllPermissions())
            {
                if (!p.IsGranted(mActivity) && p.GetCanShowAgain(mActivity))
                {
                    notGrantedPermissions.Add(p);
                }
            }

            return notGrantedPermissions;
        }

        private void UpdateCanShowAgainPermissions()
        {
            foreach (Permission p in GetAllPermissions())
            {
                if (p.IsGranted(mActivity))
                {
                    // Permission is granted. Reset the show rationale and can show again prefs.
                    p.SetCanShowAgain(mActivity, true);
                    p.ClearShowRationale(mActivity);
                    continue;
                }

                if (!p.ShouldShowRationale(mActivity))
                {
                    if (p.IsShowRationaleSet(mActivity))
                    {
                        // We were allowed to show a rationale before, but not anymore.
                        // This is a result of the "don't ask again" option selected by the user.
                        p.SetShowRationale(mActivity, false);
                        p.SetCanShowAgain(mActivity, false);
                    }
                }
                else
                {
                    // We can show a rational. This is when our permission request
                    // was shot down by the user the first time we asked.
                    p.SetShowRationale(mActivity, true);
                }
            }
        }

        private Boolean IsQPlus()
        {
            return Android.OS.Build.VERSION.SdkInt > Android.OS.BuildVersionCodes.P;
        }

        private List<Permission> GetAllPermissions()
        {
            List<Permission> list = new List<Permission>();

            list.Add(new Permission("Location",
                    IsQPlus() ? new String[] {
                        "android.permission.ACCESS_FINE_LOCATION",
                        "android.permission.ACCESS_BACKGROUND_LOCATION"} :
                            new String[] { "android.permission.ACCESS_FINE_LOCATION" },
                    LOCATION_PERMISSION_REQUEST_CODE,
                    TITLE_LOCATION, MESSAGE_LOCATION));

            if (IsQPlus())
            {
                list.Add(new Permission("Activity", new String[] { "android.permission.ACTIVITY_RECOGNITION" },
                        ACTIVITY_PERMISSION_REQUEST_CODE,
                        TITLE_ACTIVITY, MESSAGE_ACTIVITY));
            }


            return list;
        }
    }
}
