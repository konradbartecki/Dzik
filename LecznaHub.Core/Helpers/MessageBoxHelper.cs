using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LecznaHub.Core.Helpers
{
    public static class MessageBoxHelper
    {
        public static void ShowMessage(string title, string message)
        {
        #if __ANDROID__
              new AlertDialog.Builder(Application.Context)
                .SetTitle(title)
                .SetMessage(message)
                .SetPositiveButton("OK", delegate { })
                .Show();
        #elif __IOS__
              var uiAlert = new UIAlertView(title, message, null, "OK");
              uiAlert.Show();
        #elif WINDOWS_PHONE
               MessageBox.Show(title, message, MessageBoxButton.OK);
        #elif NETFX_CORE
              var dialog = new MessageDialog(title, message);
              dialog.ShowAsync();
        #endif
        }
    }
}
