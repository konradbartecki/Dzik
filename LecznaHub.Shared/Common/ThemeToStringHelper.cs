using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

namespace LecznaHub.Common
{
    public static class ThemeToStringHelper
    {
        /// <summary>
        /// Gets current application theme to string
        /// </summary>
        /// <returns>"black" or "white"</returns>
        public static string GetCurrentThemeToString()
        {
            return App.Current.RequestedTheme == ApplicationTheme.Dark ? "black" : "white";
        }
    }
}
