// Konrad Bartecki (C) 2015
// konradbartecki@outlook.com
// bartecki.org
// 
// This class consists of both native and MvvmCross ViewModel stuff so it could be a bit of a mess
// TODO: Cleanup this mess, unify stuff into MvvmCross
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Cirrious.MvvmCross.ViewModels;
using LecznaHub.Core.Helpers;
using LecznaHub.Core.Model;
using LecznaHub.Core.Providers;
using Newtonsoft.Json;
using PCLStorage;
using PCLStorage.Exceptions;

namespace LecznaHub.Core.ViewModel
{
    public class NewsViewModel : MvxViewModel
    {
        /// <summary>
        /// Init task for MvvmCross
        /// </summary>
        /// <returns></returns>
        public void Init()
        {
            Task.Run((Func<Task>) GetNewsDataAsync).Wait();
        }

        public ICommand ShowItemCommand
        {
            get
            {
                return new MvxCommand<NewsItemBase>((item) => DoShowItem(item));
            }
        }

        public void DoShowItem(NewsItemBase item)
        {
            base.ShowViewModel<DetailViewModel>(new DetailParameter() {Id = item.UniqueId});
        }

        public class DetailParameter
        {
            public string Id { get; set; }
        }

        private List<NewsProviderBase> NewsProvidersList = new List<NewsProviderBase>
        {
            new Leczna24()
        }; 

        private static NewsViewModel _sampleViewModel = new NewsViewModel();

        private ObservableCollection<NewsCollection> _groups = new ObservableCollection<NewsCollection>();
        public ObservableCollection<NewsCollection> Groups
        {
            get { return this._groups; }
        }

        public static async Task<ObservableCollection<NewsCollection>> GetGroupsAsync()
        {
            await _sampleViewModel.GetNewsDataAsync();

            return _sampleViewModel.Groups;
        }

        //public static async Task<NewsGroups> GetGroupAsync(string uniqueId)
        //{
        //    await _sampleDataSource.GetNewsDataAsync();
        //    // Simple linear search is acceptable for small data sets
        //    var matches = _sampleDataSource.Groups.Where((group) => group.Title.Equals(uniqueId));
        //    if (matches.Count() == 1) return matches.First();
        //    return null;
        //}

        public static async Task<NewsItemBase> GetItemAsync(string uniqueId)
        {
            if (string.IsNullOrWhiteSpace(uniqueId)) return null;

            await _sampleViewModel.GetNewsDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches =
                _sampleViewModel.Groups.SelectMany(group => group.Items)
                    .Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1)
            {
                var item = matches.First();
                await item.WebArticle.DownloadAsync();
                return item;
            }
            return null;
        }

        /// <summary>
        /// Downloads news collection for each available news provider
        /// </summary>
        /// <returns></returns>
        public async Task GetNewsDataAsync()
        {
            try
            {
                foreach (var provider in NewsProvidersList)
                {
                    //Download new collection of news
                    NewsCollection newsCollection = await provider.GetNewsAsync();
                    //Check if news from this provider are already stored
                    if (this.Groups.Any(x => x.Title == newsCollection.Title))
                    {
                        //Search for collection of news and replace older collections with the same title
                        for (int i = 0; i < this.Groups.Count; i++)
                        {
                            if (Groups[i].Title == newsCollection.Title)
                                Groups[i] = newsCollection;
                        }
                    }
                    else
                    {
                        //We do not have collection of news from this provider in groups
                        //so we will add this
                        this.Groups.Add(newsCollection);
                    }

                }
                //Save to local storage
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                IFolder folder = await rootFolder.CreateFolderAsync("News", CreationCollisionOption.OpenIfExists);
                IFile file = await folder.CreateFileAsync("news.json", CreationCollisionOption.ReplaceExisting);
                string json = JsonConvert.SerializeObject(Groups);
                await file.WriteAllTextAsync(json);

            }
            catch (WebException e)
            {
                //Unable to download some of the news we will fallback to the news in the local storage

                IFolder rootFolder = FileSystem.Current.LocalStorage;
                var folderExist = await rootFolder.CheckExistsAsync("News");
                if (folderExist == ExistenceCheckResult.NotFound)
                {
                    MessageBoxHelper.ShowMessage("Błąd przy pobieraniu wiadomości", "Sprawdź połączenie z internetem lub sprobuj ponownie później");
                    throw new DirectoryNotFoundException(
                        "Unable to download news and there is no stored local version to load", e);
                }
                IFolder folder = await rootFolder.GetFolderAsync("News");
                
                var FileExist = await folder.CheckExistsAsync("news.json");
                if (FileExist == ExistenceCheckResult.FileExists)
                {
                    var file = await folder.GetFileAsync("news.json");
                    var json = await file.ReadAllTextAsync();
                    _groups = JsonConvert.DeserializeObject<ObservableCollection<NewsCollection>>(json);
                }

                //DzikMessenger dzikMessenger = new DzikMessenger();
                //DzikWebArticleItem dzikWebArticle = new DzikWebArticleItem("", dzikMessenger);
                //DzikArticleItem item = new DzikArticleItem("", "Nie udało się pobrać wiadomości", "", "Sprawdź połączenie z internetem lub spróbuj ponownie później", dzikWebArticle, dzikMessenger);
                //if (this.Groups.Count == 0)
                //{
                //    this.Groups.Add(new NewsCollection("Dzik informacje"));
                //    this.Groups[0].Items.Add(item);
                //}
                //else
                //{
                //    this.Groups[0].Items.Insert(0, item);
                //}              
                //Debug.WriteLine("Exception while downloading news");
            }


        }
    }
}
