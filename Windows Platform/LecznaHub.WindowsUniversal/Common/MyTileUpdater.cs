using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using LecznaHub.Core.Model;

namespace LecznaHub.Common
{
    public static class MyTileUpdater
    {
        private static TileUpdater _updater;

        public static async Task UpdateTile(NewsCollection newsCollection)
        {
            // Create a tile update manager for the specified syndication feed.
            _updater = TileUpdateManager.CreateTileUpdaterForApplication();
            _updater.EnableNotificationQueue(true);
            _updater.Clear();

            // Keep track of the number feed items that get tile notifications. 
            int itemCount = 0;

            // Create a tile notification for each feed item.

            foreach (var item in newsCollection.Items)
            {
                //We need to download article for each item to get url for HD image
                await item.WebArticle.DownloadAsync();

                CreateMediumTile(item);
                CreateWideTile(item);
                CreateLargeTile(item);

                // Don't create more than 5 notifications.
                if (itemCount++ > 3) break;
            }
            Debug.WriteLine("Live tile update completed");
        }
        //tile creation functions inb4 DRY
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

        private static void CreateLargeTile(NewsItemBase item)
        {
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare310x310ImageAndText01);
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
