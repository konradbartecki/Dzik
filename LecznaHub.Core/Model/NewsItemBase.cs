using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using LecznaHub.Core.Providers;

namespace LecznaHub.Core.Model
{
    /// <summary>
    /// Generic item data model. To be inheirted by news provider
    /// </summary>
    [DataContract]
    public class NewsItemBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <param name="title"></param>
        /// <param name="imagePath"></param>
        /// <param name="description"></param>
        /// <param name="webArticle">[NotNull] This field cannot be null - please create new article with </param>
        /// <param name="newsProvider"></param>
        public NewsItemBase(string uniqueId, string title, string imagePath, string description, WebArticleBase webArticle, NewsProviderBase newsProvider)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Description = description;
            this.ImagePath = imagePath;
            this.WebArticle = webArticle;
            this.NewsProvider = newsProvider;
            if(WebArticle == null)
                Debug.WriteLine("Warning: NewItemBase created without webArticle (null)");
        }

        [DataMember]
        public string UniqueId { get; private set; }
        [DataMember]
        public string Title { get; private set; }
        [DataMember]
        public string Description { get; private set; }
        [DataMember]
        public string ImagePath { get; private set; }
        [DataMember]
        public string ProviderNamespace { get; private set; }
        [DataMember]
        public NewsProviderBase NewsProvider { get; private set; }
        [DataMember]
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
