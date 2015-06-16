﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LecznaHub.Core.Model
{
    /// <summary>
    /// Collection of news items from given provider
    /// </summary>
    public class NewsCollection
    {
        public NewsCollection(string title)
        {
            //Contains name of news provider
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
