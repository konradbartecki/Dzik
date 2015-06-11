using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LecznaHub.Core.Model
{
    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class NewsCollection
    {
        public NewsCollection(string title)
        {
            this.Title = title;
            this.Items = new ObservableCollection<NewsItemBase>();
        }

        public string Title { get; private set; }
        public ObservableCollection<NewsItemBase> Items { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
