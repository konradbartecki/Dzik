using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LecznaHub.Core.Providers;

namespace LecznaHub.Core.Model
{
    /// <summary>
    /// Generic item data model. To be inheirted by news provider
    /// </summary>
    public class NewsItemBase
    {
        public NewsItemBase(string uniqueId, string title, string imagePath, string description, WebArticleBase webArticle, NewsProviderBase newsProvider)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Description = description;
            this.ImagePath = imagePath;
            this.WebArticle = webArticle;
            this.NewsProvider = newsProvider;
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        //public Bitmap BitmapImage => GetImageBitmapFromUrl(ImagePath);
        public string ProviderNamespace { get; private set; }
        public NewsProviderBase NewsProvider { get; private set; }
        public WebArticleBase WebArticle { get; private set; }

        //public virtual Task DownloadWebArticleTask()
        //{
        //    Downloader downloader = new Downloader(new Uri(UniqueId));
        //    WebArticleBase webArticle = new WebArticleBase();
        //}

        public override string ToString()
        {
            return this.Title;
        }
}
}
