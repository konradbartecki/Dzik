﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;
using LecznaHub.Core.Helpers;
using LecznaHub.Core.Providers;
using System.IO;
using System.Runtime.Serialization;

namespace LecznaHub.Core.Model
{
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
        public NewsProviderBase Provider { get; private set;}

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
            //HtmlDocument htmldoc = new HtmlDocument();
            return String.Format("<img src=\"{3}\");\"><h1>{0}</h1><h2>{1}</h2>{2}",
                this.Title, this.Headline, this.ArticleBody, this.ImagePath);
            //var node = HtmlNode.CreateNode("");
            //htmldoc.DocumentNode.AppendChild(node);

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
