using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using LecznaHub.Core.Helpers;
using LecznaHub.Core.Model;
using LecznaHub.Core.Model.News;
using Newtonsoft.Json;
using PCLStorage;
using PCLStorage.Exceptions;

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
            //UniversalNewsItemStore savedStore = new UniversalNewsItemStore();
            UniversalNewsItemStore savedStore = await GetSavedNewsStoreAsync();
            if (savedStore != null)
                this.NewsStore = savedStore;
            else
                NewsStore = new UniversalNewsItemStore();
            await RefreshItemStoreAsync();
            FilterItems();
            IsInitialized = true;
        }

        private async Task<UniversalNewsItemStore> GetSavedNewsStoreAsync()
        {
            var folder = await StorageHelper.GetNewsFolderAsync();
            if (folder == null) return null;
            var fileExist = await folder.CheckExistsAsync(Config.NewsDataStoreFilename);
            if (fileExist != ExistenceCheckResult.FileExists) return null;
            var file = await folder.GetFileAsync(Config.NewsDataStoreFilename);
            var json = await file.ReadAllTextAsync();
            try
            {
                UniversalNewsItemStore store = JsonConvert.DeserializeObject<UniversalNewsItemStore>(json);
                return store;
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private async Task SaveNewsStoreAsync()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync(Config.NewsStoreFolderName, CreationCollisionOption.OpenIfExists);
            IFile file = await folder.CreateFileAsync(Config.NewsDataStoreFilename, CreationCollisionOption.ReplaceExisting);
            try
            {
                string json = JsonConvert.SerializeObject(this.NewsStore);
                await file.WriteAllTextAsync(json);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }                       
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
                    var collection = new UniversalNewsCollection(newsCollection.Title);
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
            List<UniversalNewsItem> itemsToAdd = new List<UniversalNewsItem>();
            List<UniversalNewsItem> combinedList;
            foreach (var item in oldCollection.Items)
            {
                if (universalNewsCollection.Items.Any(x => x.Title == item.Title))
                    //we already have this item converted
                    continue;
                //this is a new item that needs to be added
                var convertedItem = await convertNewsitemAsync(item);
                //TODO: Item order bug?
                itemsToAdd.Add(convertedItem);
                IsSaveNeeded = true;
            }
            if (IsSaveNeeded)
            {
                combinedList = itemsToAdd.Concat(universalNewsCollection.Items).ToList();
                universalNewsCollection.Items = combinedList;
            }
            return IsSaveNeeded;
        }

        private async Task<UniversalNewsItem> convertNewsitemAsync(NewsItemBase item)
        {
            await item.WebArticle.DownloadAsync();
            var universalitem = new UniversalNewsItem(item);
            await universalitem.CacheImageAsync();
            return universalitem;
        }

        public void FilterItems()
        {
            FilteredNewsItems.Clear();
            var itemsToAdd = NewsStore.NewsCollections.SelectMany(x => x.Items);
            foreach (var universalNewsItem in itemsToAdd)
            {
                FilteredNewsItems.Add(universalNewsItem);
            }
        }

    }
}
