using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Calls;
using Windows.System;

namespace LecznaHub.Common
{
    public static class LauncherHelpers
    {
        public static async void ShowReviewDialog()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + Windows.ApplicationModel.Package.Current.Id));
        }

        //public static void ShowCallDialog(string number, string name)
        //{
        //    PhoneCallManager.ShowPhoneCallUI(number, name);
        //}
    }
}
