using System;
using Android.App;
using Android.Content;
using Android.Support.V4.App;

namespace SDKStarter
{
    public class Permission
    {
        private static readonly string KEY_CAN_SHOW_AGAIN = "can_show_again";
        private static readonly string KEY_SHOW_RATIONALE = "show_rationale";

        private readonly String name;
        private readonly String[] manifestPermissions;
        private readonly int askCode;
        private readonly String dialogTitle;
        private readonly String dialogMessage;

        public Permission(String name, String[] manifestPermissions, int askCode,
            String dialogTitle, String dialogMessage)
        {
            this.name = name;
            this.manifestPermissions = manifestPermissions;
            this.askCode = askCode;
            this.dialogTitle = dialogTitle;
            this.dialogMessage = dialogMessage;
        }

        public Boolean IsGranted(Activity activity)
        {
            foreach (String permission in manifestPermissions)
            {
                if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(activity, permission)
                        != Android.Content.PM.Permission.Granted)
                {
                    return false;
                }
            }
            return true;
        }

        public Boolean ShouldShowRationale(Activity activity)
        {
            foreach (String permission in manifestPermissions)
            {
                if (ActivityCompat.ShouldShowRequestPermissionRationale(activity, permission))
                {
                    return true;
                }
            }
            return false;
        }

        public int GetAskCode()
        {
            return askCode;
        }

        public String[] GetManifestPermissions()
        {
            return manifestPermissions;
        }

        public String GetDialogTitle()
        {
            return dialogTitle;
        }

        public String GetDialogMessage()
        {
            return dialogMessage;
        }

        override public String ToString()
        {
            return "Permission{" +
                    "name='" + name + '\'' +
                    '}';
        }

        public Boolean GetCanShowAgain(Activity activity)
        {
            String key = KEY_CAN_SHOW_AGAIN + "_" + askCode;
            return GetPrefs(activity).GetBoolean(key, true);
        }

        public void SetCanShowAgain(Activity activity, Boolean value)
        {
            String key = KEY_CAN_SHOW_AGAIN + "_" + askCode;
            GetPrefs(activity).Edit().PutBoolean(key, value).Apply();
        }

        public  Boolean IsShowRationaleSet(Activity activity)
        {
            String key = KEY_SHOW_RATIONALE + "_" + askCode;
            return GetPrefs(activity).Contains(key);
        }

        public void SetShowRationale(Activity activity, Boolean value)
        {
            String key = KEY_SHOW_RATIONALE + "_" + askCode;
            GetPrefs(activity).Edit().PutBoolean(key, value).Apply();
        }

        public void ClearShowRationale(Activity activity)
        {
            String key = KEY_SHOW_RATIONALE + "_" + askCode;
            GetPrefs(activity).Edit().Remove(key).Apply();
        }

        private ISharedPreferences GetPrefs(Activity activity)
        {
            return activity.GetSharedPreferences("permission", FileCreationMode.Private);
        }
    }
}
