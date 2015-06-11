using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        protected XDocument WebPageXDocument { get; set; }
        /// <summary>
        /// Also contains all data that can be used to build WebArticle like title, headline, etc.
        /// </summary>
        public string HtmlPage { get; private set; }

        public async Task DownloadAsync()
        {
            Downloader downloader = new Downloader(new Uri(UniqueID));
            HtmlPage = await downloader.GetPageAsync();

            WebPageXDocument = XDocument.Parse(HtmlPage);

            Title = GetTitle();
            Headline = GetHeadline();
            ImagePath = GetImagePath();
            HtmlPage = PrepareHtmlPage();
        }

        protected abstract string GetHeadline();

        protected abstract string GetImagePath();

        protected abstract string PrepareHtmlPage();

        protected abstract string GetTitle();
    }
}
