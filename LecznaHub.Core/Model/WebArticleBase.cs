using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;
using LecznaHub.Core.Helpers;

namespace LecznaHub.Core.Model
{
    public abstract class WebArticleBase
    {
        public WebArticleBase(string uniqueId)
        {
            this.UniqueID = uniqueId;
            this.IsPrepared = false;
        }

        public bool IsPrepared { get; protected set; }
        public string UniqueID { get; private set; }
        public string Title { get; private set; }
        public string Headline { get; private set; }
        public string ImagePath { get; private set; }
        protected HtmlDocument DownloadedHtmlDocument { get; set; }
        public string ArticleBody { get; private set; }
        public string FormattedHtmlDocument { get; private set; }

        /// <summary>
        /// Override this class if you need custom downloader with custom encoding
        /// </summary>
        /// <returns></returns>
        public virtual Downloader CreateNewDownloader()
        {
            return new Downloader(new Uri(this.UniqueID));
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
            //HtmlDocument htmldoc = new HtmlDocument();
            return String.Format("<html><head></head><body><h1>{0}</h1><h2>{1}</h2>{2}</body></html>",
                this.Title, this.Headline, this.ArticleBody);
            //var node = HtmlNode.CreateNode("");
            //htmldoc.DocumentNode.AppendChild(node);

        }

        protected abstract string GetHeadline();

        protected abstract string GetImagePath();

        protected abstract string GetArticleBody();

        protected abstract string GetTitle();

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(FormattedHtmlDocument))
                return "";
            return FormattedHtmlDocument;
        }
    }
}
