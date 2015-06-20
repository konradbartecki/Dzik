using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Calls;
//using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Store;
using Windows.System;

namespace LecznaHub.Common
{
    public static class LauncherHelpers
    {
        public static async void ShowReviewDialog()
        {
            var uri = new Uri(string.Format("ms-windows-store:reviewapp?appid={0}", CurrentApp.AppId));
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        public static async void ShowMoreApps()
        {
            var uri = new Uri(string.Format(@"ms-windows-store:search?keyword={0}", "Konrad Bartecki"));
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        public static async void ShareApp()
        {
            var uri = new Uri(string.Format("ms-windows-store:navigate?appid={0}", CurrentApp.AppId));
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        //public static void ShowCallDialog(string number, string name)
        //{
        //    PhoneCallManager.ShowPhoneCallUI(number, name);
        //}

        //public static async void ComposeSugestionEmail()
        //{
        //    EmailRecipient sendTo = new EmailRecipient()
        //    {
        //        Address = "konradbartecki@outlook.com"
        //    };

        //    //generate mail object
        //    EmailMessage mail = new EmailMessage();
        //    mail.Subject = "Sugestie dot. aplikacji Łęczna";

        //    //add recipients to the mail object
        //    mail.To.Add(sendTo);
        //    //mail.Bcc.Add(sendTo);
        //    //mail.CC.Add(sendTo);

        //    //open the share contract with Mail only:
        //    await EmailManager.ShowComposeNewEmailAsync(mail);
        //}
    }
}
