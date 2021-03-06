﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Xml.Linq;
using Windows.ApplicationModel.Calls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LecznaHub.Core;
using LecznaHub.Core.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace LecznaHub.TestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            //Downloader downloader = new Downloader(new Uri("http://leczna24.pl/rss/informacje_utf8.php"));
            //string s = await downloader.GetNewsAsync();

            //XDocument feedDocument = XDocument.Parse(s);
            //var news = feedDocument.Descendants("item");

            //MessageDialog msgDialog = new MessageDialog("Done", "Downloader");
            //msgDialog.ShowAsync();
        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            //NewsDataSource defaultDataSource = new NewsDataSource();
            //defaultDataSource.GetNewsDataAsync();
            var statusBar = StatusBar.GetForCurrentView();
            var progressind = statusBar.ProgressIndicator;
            progressind.Text = "Downloading...";
            await progressind.ShowAsync();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        { 
            PhoneCallManager.ShowPhoneCallUI("+48721238100", "Konrad");
        }
    }
}
