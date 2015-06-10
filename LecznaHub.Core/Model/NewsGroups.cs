using System;
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
    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class NewsItem
    {
        public NewsItem(string uniqueId, string title, string imagePath, string description)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Description = description;
            this.ImagePath = imagePath;
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class NewsGroups
    {
        public NewsGroups(string title)
        {
            this.Title = title;
            this.Items = new ObservableCollection<NewsItem>();
        }

        public string Title { get; private set; }
        public ObservableCollection<NewsItem> Items { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    public class NewsDataSource
    {
        private static NewsDataSource _sampleDataSource = new NewsDataSource();

        private ObservableCollection<NewsGroups> _groups = new ObservableCollection<NewsGroups>();
        public ObservableCollection<NewsGroups> Groups
        {
            get { return this._groups; }
        }

        public static async Task<IEnumerable<NewsGroups>> GetGroupsAsync()
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

        //public static async Task<NewsGroups> GetItemAsync(string uniqueId)
        //{
        //    await _sampleDataSource.GetNewsDataAsync();
        //    // Simple linear search is acceptable for small data sets
        //    var matches =
        //        _sampleDataSource.Groups.SelectMany(group => group.Items)
        //            .Where((item) => item.UniqueId.Equals(uniqueId));
        //    if (matches.Count() == 1) return matches.First();
        //    return null;
        //}

        public async Task GetNewsDataAsync()
        {
            this.Groups.Add(await Leczna24.GetNewsAsync());
            Debug.WriteLine("Reading news done");

        }
    }
}
