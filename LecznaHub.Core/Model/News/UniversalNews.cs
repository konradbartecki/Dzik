using System;
using System.Collections.Generic;
using LecznaHub.Core.Helpers;

namespace LecznaHub.Core.Model.News
{
    /// <summary>
    /// Provider independent fully downloaded universal news item that can be stored locally
    /// </summary>
    public class UniversalNewsItem
    {
        //Work in progress
        /// <summary>
        /// HTTP Link to the full article
        /// </summary>
        public string UniqueId { get; set; }
        public string Title { get; set; }
        public string ProviderName { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string DownloadedArticleHtml { get; set; }
        public Uri WebsiteArticleUri => new Uri(this.UniqueId);

        public override string ToString()
        {
            return Title;
        }

        public UniversalNewsItem()
        {
            
        }

        public UniversalNewsItem(NewsItemBase newsBase)
        {
            UniqueId = newsBase.UniqueId;
            Title = newsBase.Title;
            Description = newsBase.Description;
            ProviderName = newsBase.NewsProvider.Name;
            //TODO: Check formatted html article theme against current theme
            DownloadedArticleHtml = newsBase.WebArticle.FormattedHtmlDocument;
            ImagePath = newsBase.WebArticle.ImagePath;
        }
    }

    public class UniversalNewsCollection
    {
        public string ProviderName { get; set; }
        public List<UniversalNewsItem> Items { get; set; }

        public UniversalNewsCollection(string name)
        {
            this.ProviderName = name;
            this.Items = new List<UniversalNewsItem>();
        }

        public UniversalNewsCollection()
        {
            this.Items = new List<UniversalNewsItem>();
        }
    }

    public class UniversalNewsItemStore
    {
        public string Version { get; set; }

        public List<UniversalNewsCollection> NewsCollections { get; set; }

        public UniversalNewsItemStore()
        {
            this.Version = AssemblyVersionHelper.GetAssemblyVersion(this);
            this.NewsCollections = new List<UniversalNewsCollection>();
        }

    }
}
