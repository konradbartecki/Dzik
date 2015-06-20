using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace LecznaHub.Common
{
    public static class ProgressIndicator
    {
        public static void ShowLoader(string loaderMessage, bool showProgress)
        {
            StatusBarProgressIndicator progressbar = StatusBar.GetForCurrentView().ProgressIndicator;         
            if (showProgress)
            {
                progressbar.Text = loaderMessage;
                progressbar.ShowAsync();
            }
            else
            {
                progressbar.HideAsync();
            }
        }
    }
}
