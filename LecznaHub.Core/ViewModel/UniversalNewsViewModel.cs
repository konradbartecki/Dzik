﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using LecznaHub.Core.Model;
using LecznaHub.Core.Model.News;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using PCLStorage;
using PCLStorage.Exceptions;
using Xamarin;

namespace LecznaHub.Core.ViewModel
{
    public class UniversalNewsViewModel : MvxViewModel
    {
        public ObservableCollection<UniversalNewsItem> FilteredNewsItems { get; set; }
        public UniversalNewsItemStore NewsStore { get; set; }

        private bool IsInitialized;
        public UniversalNewsViewModel(ObservableCollection<UniversalNewsCollection> newsStore)
        {

        }

        public UniversalNewsViewModel()
        {
            IsInitialized = false;
        }

        public async Task InitializeAsync()
        {
            var savedStore = await GetSavedNewsStoreAsync();
            if (savedStore != null)
                this.NewsStore = savedStore;
            await RefreshItemStoreAsync();
            IsInitialized = true;
        }

        private async Task<UniversalNewsItemStore> GetSavedNewsStoreAsync()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            var folderExist = await rootFolder.CheckExistsAsync(Config.NewsStoreFolderName);
            if (folderExist == ExistenceCheckResult.NotFound)
                return null;
            IFolder folder = await rootFolder.GetFolderAsync(Config.NewsStoreFolderName);

            var fileExist = await folder.CheckExistsAsync(Config.NewsDataStoreFilename);
            if (fileExist != ExistenceCheckResult.FileExists) return null;
            var file = await folder.GetFileAsync(Config.NewsDataStoreFilename);
            var json = await file.ReadAllTextAsync();
            try
            {
                return JsonConvert.DeserializeObject<UniversalNewsItemStore>(json);
            }
            catch (JsonException jsonException)
            {
                var telemetry = new TelemetryClient();
                telemetry.TrackException(jsonException);
                return null;
            }
        }

        private async Task SaveNewsStoreAsync()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync(Config.NewsStoreFolderName, CreationCollisionOption.OpenIfExists);
            IFile file = await folder.CreateFileAsync(Config.NewsDataStoreFilename, CreationCollisionOption.ReplaceExisting);
            string json = JsonConvert.SerializeObject(this.NewsStore);
            await file.WriteAllTextAsync(json);
        }

        public async Task RefreshItemStoreAsync()
        {
            bool IsSaveNeeded = false;

            foreach (var provider in Config.News.NewsProvidersList)
            {
                //Download new collection of news
                NewsCollection newsCollection = await provider.GetNewsAsync();
                //Check if news from this provider are already stored
                if (this.NewsStore.NewsCollections.Any(x => x.ProviderName == newsCollection.Title))
                {
                    //Search for collection of news and replace older collections with the same title
                    for (int i = 0; i < this.NewsStore.NewsCollections.Count; i++)
                    {
                        if (NewsStore.NewsCollections[i].ProviderName == newsCollection.Title)
                            IsSaveNeeded = await ConvertCollection(newsCollection, NewsStore.NewsCollections[i]);
                    }
                }
                else
                {
                    //We do not have collection of news from this provider in groups
                    //so we will add this
                    var collection = new UniversalNewsCollection();
                    collection.ProviderName = newsCollection.Title;

                    IsSaveNeeded = await ConvertCollection(newsCollection, collection);
                    this.NewsStore.NewsCollections.Add(collection);
                }
            }

            if (IsSaveNeeded)
                await SaveNewsStoreAsync();
        }

        /// <summary>
        /// Refreshes or downloads item store
        /// </summary>
        /// <param name="oldCollection">Old collection from where to grab items</param>
        /// <param name="universalNewsCollection">New universal type collection where to store new items</param>
        /// <returns>True if new news collection was changed and should be saved to the news store</returns>
        private async Task<bool> ConvertCollection(NewsCollection oldCollection,
            UniversalNewsCollection universalNewsCollection)
        {
            bool IsSaveNeeded = false;

            foreach (var item in oldCollection.Items)
            {
                if (universalNewsCollection.Items.Any(x => x.Title == item.Title))
                    //we already have this item converted
                    continue;
                //this is a new item that needs to be added
                var convertedItem = await convertNewsitemAsync(item);
                //TODO: Item order bug?
                universalNewsCollection.Items.Insert(0, convertedItem);
                IsSaveNeeded = true;
            }
            return IsSaveNeeded;
        }

        private async Task<UniversalNewsItem> convertNewsitemAsync(NewsItemBase item)
        {
            await item.WebArticle.DownloadAsync();
            return new UniversalNewsItem(item);
        }

    }
}