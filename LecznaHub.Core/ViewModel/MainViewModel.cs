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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Cirrious.MvvmCross.ViewModels;
using LecznaHub.Core.Model;
using LecznaHub.Core.Providers;

namespace LecznaHub.Core.ViewModel
{
    public class MainViewModel : MvxViewModel
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

        private static MainViewModel _sampleViewModel = new MainViewModel();

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
            await _sampleViewModel.GetNewsDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches =
                _sampleViewModel.Groups.SelectMany(group => group.Items)
                    .Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1)
            {
                var item = matches.First();
                if(item.WebArticle == null)
                    //will download article if it's not downloaded yet
                    await item.WebArticle.DownloadAsync();
                return item;
            }
            return null;
        }

        public async Task GetNewsDataAsync()
        {
            foreach (var provider in NewsProvidersList)
            {
                //Download new collection of news
                NewsCollection newsCollection = await provider.GetNewsAsync();
                //Check if news from this provider are already stored
                if (this.Groups.Any(x => x.Title == newsCollection.Title))
                {
                    //Search for collection of news and replace
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
            Debug.WriteLine("Reading news done");

        }
    }
}
