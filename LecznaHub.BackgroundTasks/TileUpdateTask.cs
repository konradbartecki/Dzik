using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using LecznaHub.Core.Model;
using LecznaHub.Core.ViewModel;

namespace LecznaHub.BackgroundTasks
{
    public class TileUpdateTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            Debug.WriteLine("Task fired");

            var feed = await MainViewModel.GetGroupsAsync();

            // Create a tile update manager for the specified syndication feed.
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();

            // Keep track of the number feed items that get tile notifications. 
            int itemCount = 0;

            // Create a tile notification for each feed item.
            foreach (var item in feed[0].Items)
            {
                XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text03);

                var title = item.Title;
                //string titleText = title.Text == null ? String.Empty : title.Text;
                tileXml.GetElementsByTagName("text")[0].InnerText = title;

                // Create a new tile notification. 
                updater.Update(new TileNotification(tileXml));

                // Don't create more than 5 notifications.
                if (itemCount++ > 5) break;
            }

            deferral.Complete();
        }
    }

    public static class TileHelper
    {
        //private const string taskName = "TileUpdateTask";

        public static async void RegisterBackgroundTask()
        {

        }
        }
    }
