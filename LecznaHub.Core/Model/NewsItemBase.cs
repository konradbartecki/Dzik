using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
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

    [DataContract]
    public abstract class WebArticleBase
    {
        protected WebArticleBase(string uniqueId, NewsProviderBase provider)
        {
            this.UniqueId = uniqueId;
            this.IsPrepared = false;
            this.Provider = provider;
        }

        public bool IsPrepared { get; protected set; }
        [DataMember]
        public string UniqueId { get; private set; }
        [DataMember]
        public string Title { get; private set; }
        [DataMember]
        public string Headline { get; private set; }
        [DataMember]
        public string ImagePath { get; private set; }
        protected HtmlDocument DownloadedHtmlDocument { get; set; }
        [DataMember]
        public string ArticleBody { get; private set; }
        [DataMember]
        public string FormattedHtmlDocument { get; private set; }
        public NewsProviderBase Provider { get; private set; }

        //[OnDeserialized]
        //internal void Initialize(StreamingContext context)
        //{
        //    this.car is already in place, we've been deserialized
        //    Debug.WriteLine("test");
        //}

        /// <summary>
        /// Override this class if you need custom downloader with custom encoding
        /// </summary>
        /// <returns></returns>
        public virtual Downloader CreateNewDownloader()
        {
            return new Downloader(new Uri(this.UniqueId));
        }

        public async Task DownloadAsync()
        {
            Downloader downloader = CreateNewDownloader();
            string download = await downloader.GetPageAsync();

            DownloadedHtmlDocument = new HtmlDocument();
            DownloadedHtmlDocument.LoadHtml(download);

            Title = GetTitle();
            Headline = GetHeadline();
            ImagePath = GetImagePath();
            ArticleBody = GetArticleBody();
            FormattedHtmlDocument = BuildHtmlPage();
        }

        public string BuildHtmlPage()
        {
            return String.Format("<img src=\"{3}\");\"><h1>{0}</h1><h2>{1}</h2>{2}",
                this.Title, this.Headline, this.ArticleBody, this.ImagePath);
        }

        protected abstract string GetHeadline();

        protected abstract string GetImagePath();

        protected abstract string GetArticleBody();

        protected abstract string GetTitle();

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(FormattedHtmlDocument) ? "" : FormattedHtmlDocument;
        }
    }
}
