using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Calls;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Store;
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

        public static async void MyButtonHandler()
        {
            EmailRecipient sendTo = new EmailRecipient()
            {
                Address = "konradbartecki@outlook.com"
            };

            //generate mail object
            EmailMessage mail = new EmailMessage();
            mail.Subject = "Sugestie dot. aplikacji Łęczna";

            //add recipients to the mail object
            mail.To.Add(sendTo);
            //mail.Bcc.Add(sendTo);
            //mail.CC.Add(sendTo);

            //open the share contract with Mail only:
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }

        //private async void Grid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        //{
        //    review
        //    var uri = new Uri(string.Format("ms-windows-store:reviewapp?appid={0}", CurrentApp.AppId));
        //    await Windows.System.Launcher.LaunchUriAsync(uri);
        //}

        //private void Grid_Tapped_1(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        //{
        //    About
        //    if (!Frame.Navigate(typeof(AboutPage)))
        //    {
        //        throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
        //    }
        //}

        //private async void moreApps_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        //{
        //    var uri = new Uri(string.Format(@"ms-windows-store:search?keyword={0}", "Konrad Bartecki"));
        //    await Windows.System.Launcher.LaunchUriAsync(uri);
        //}

        //private async void Share_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        //{
        //    var uri = new Uri(string.Format("ms-windows-store:navigate?appid={0}", CurrentApp.AppId));
        //    await Windows.System.Launcher.LaunchUriAsync(uri);
        //}
    }
}
