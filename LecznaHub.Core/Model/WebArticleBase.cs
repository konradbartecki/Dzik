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
        protected HtmlDocument HtmlDocument { get; set; }
        /// <summary>
        /// Also contains all data that can be used to build WebArticle like title, headline, etc.
        /// </summary>
        public string HtmlPage { get; private set; }

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
            HtmlPage = await downloader.GetPageAsync();
            //var webdl = new HtmlWeb();
            //var HtmlDocument = await webdl.LoadFromWebAsync(UniqueID, Encoding.GetEncoding("iso-8859-2"));
            HtmlPage = ConvertEncoding(HtmlPage);

            HtmlDocument = new HtmlDocument();
            HtmlDocument.LoadHtml(HtmlPage);
            //WebPageXDocument = XDocument.Parse(HtmlPage);

            Title = GetTitle();
            Headline = GetHeadline();
            ImagePath = GetImagePath();
            HtmlPage = PrepareHtmlPage();



        }

        protected abstract string GetHeadline();

        protected abstract string GetImagePath();

        protected abstract string PrepareHtmlPage();

        protected abstract string GetTitle();

        protected virtual string ConvertEncoding(string data)
        {
            return data;
        }
    }
}
