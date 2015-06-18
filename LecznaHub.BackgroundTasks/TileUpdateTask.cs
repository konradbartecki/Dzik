using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using LecznaHub.Core.Model;
using LecznaHub.Core.ViewModel;

namespace LecznaHub.BackgroundTasks
{
    public sealed class TileUpdateTask : IBackgroundTask
    {
        private static TileUpdater _updater;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            Debug.WriteLine("Task fired");

            var feed = await MainViewModel.GetGroupsAsync();

            UpdateTile(feed);

            deferral.Complete();
        }

        public static async void UpdateTile(ObservableCollection<NewsCollection> feed)
        {
            // Create a tile update manager for the specified syndication feed.
            _updater = TileUpdateManager.CreateTileUpdaterForApplication();
            _updater.EnableNotificationQueue(true);
            _updater.Clear();

            // Keep track of the number feed items that get tile notifications. 
            int itemCount = 0;

            // Create a tile notification for each feed item.
            foreach (var item in feed[0].Items)
            {
                await item.WebArticle.DownloadAsync();

                CreateMediumTile(item);
                CreateWideTile(item);

                // Don't create more than 5 notifications.
                if (itemCount++ > 5) break;
            }
        }

        private static void CreateWideTile(NewsItemBase item)
        {
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150ImageAndText01);
            //string titleText = title.Text == null ? String.Empty : title.Text;
            tileXml.GetElementsByTagName("image")[0].Attributes[1].NodeValue = item.WebArticle.ImagePath;
            tileXml.GetElementsByTagName("text")[0].InnerText = item.Title;
            //tileXml.GetElementsByTagName("text")[1].InnerText = item.Description;

            var s = tileXml.GetXml();
            // Create a new tile notification. 
            _updater.Update(new TileNotification(tileXml));
        }

        private static void CreateMediumTile(NewsItemBase item)
        {
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150PeekImageAndText04);
            //string titleText = title.Text == null ? String.Empty : title.Text;
            tileXml.GetElementsByTagName("image")[0].Attributes[1].NodeValue = item.WebArticle.ImagePath;
            tileXml.GetElementsByTagName("text")[0].InnerText = item.Title;
            //tileXml.GetElementsByTagName("text")[1].InnerText = item.Description;

            var s = tileXml.GetXml();
            // Create a new tile notification. 
            _updater.Update(new TileNotification(tileXml));
        }
    }
}
