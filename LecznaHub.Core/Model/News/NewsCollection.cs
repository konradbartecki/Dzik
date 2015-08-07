using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LecznaHub.Core.Model
{
    /// <summary>
    /// Collection of news items from given provider
    /// </summary>
    [DataContract]
    public class NewsCollection
    {
        public NewsCollection(string title)
        {
            //Contains name of news provider
            this.Title = title;
            this.Items = new ObservableCollection<NewsItemBase>();
        }
        [DataMember]
        public string Title { get; private set; }
        [DataMember]
        public ObservableCollection<NewsItemBase> Items { get; private set; }

        public async Task DownloadAllArticlesAsync()
        {
            foreach (var item in Items)
            {
                if (item.WebArticle != null) await item.WebArticle.DownloadAsync();
            }
        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
