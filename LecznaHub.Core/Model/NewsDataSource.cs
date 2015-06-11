﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LecznaHub.Core.Providers;

namespace LecznaHub.Core.Model
{
    public class NewsDataSource
    {
        private List<NewsProviderBase> NewsProvidersList = new List<NewsProviderBase>
        {
            new Leczna24()
        }; 

        private static NewsDataSource _sampleDataSource = new NewsDataSource();

        private ObservableCollection<NewsCollection> _groups = new ObservableCollection<NewsCollection>();
        public ObservableCollection<NewsCollection> Groups
        {
            get { return this._groups; }
        }

        public static async Task<IEnumerable<NewsCollection>> GetGroupsAsync()
        {
            await _sampleDataSource.GetNewsDataAsync();

            return _sampleDataSource.Groups;
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
            //await _sampleDataSource.GetNewsDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches =
                _sampleDataSource.Groups.SelectMany(group => group.Items)
                    .Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1)
            {
                var item = matches.First();
                await item.WebArticle.DownloadAsync();
                return item;
            }
            return null;
        }

        public async Task GetNewsDataAsync()
        {
            foreach (var provider in NewsProvidersList)
            {
                this.Groups.Add(await provider.GetNewsAsync());
            }
            //commented because I've implememted multiple providers support above
            //this.Groups.Add(await Leczna24.GetNewsAsync());
            Debug.WriteLine("Reading news done");

        }
    }
}
