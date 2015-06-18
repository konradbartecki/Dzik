using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using HtmlAgilityPack;
using LecznaHub.Core.Helpers;
using LecznaHub.Core.Model;

namespace LecznaHub.Core.Providers
{
    public class Leczna24 : NewsProviderBase
    {
        public Leczna24() : base ("Łęczna24", "http://leczna24.pl", new Uri("http://leczna24.pl/rss/informacje_utf8.php"))
        {

        }

        //Read as XML and select all items
        public override NewsCollection GetNewsFromDownloadedData(string data)
        {
            //TODO: Reuse (if needed) XML RSS reading
            XDocument newsXmlDocument = XDocument.Parse(data);
            var XmlItems = newsXmlDocument.Descendants("item");
            //Cast items into model
            NewsCollection collection = new NewsCollection("Leczna24 news");
            foreach (XElement item in XmlItems)
            {
                string id = item.Element("link").Value;
                string title = item.Element("title").Value;
                string description = item.Element("description").Value;

                collection.Items.Add(new Leczna24NewsItem(id, title, description, this));
            }
            return collection;
        }

    }

    public class Leczna24NewsItem : NewsItemBase
    {
        /// <summary>
        /// Łęczna24 news class
        /// </summary>
        /// <param name="uniqueId">News ID and/or link</param>
        /// <param name="title">News title</param>
        /// <param name="imagePath">Not used because img path is contained in description</param>
        /// <param name="description">Contains longer description about news</param>
        public Leczna24NewsItem(string uniqueId, string title, string description, NewsProviderBase provider)
            : base (uniqueId, title, GetImagePath(description), GetDescription(description), new Leczna24WebArticle(uniqueId, provider), provider)
        {
            //Will pass UniqueID and Title parameter back to base class 
            //but we will manipulate ImagePath and Description
        }

        private static string GetImagePath(string data)
        {
            //we could have bad performance here
            string s = data.Replace("<img src=\"", "");
            int i = s.IndexOf("\"", StringComparison.Ordinal);
            s = s.Remove(i);
            return s;
        }

        private static string GetDescription(string data)
        {
            int i = data.IndexOf("<br />", StringComparison.Ordinal);
            string s = data.Remove(0, i);
            s = s.Replace("<br /> ", "");
            return s;
        }
    }

    [DataContract]
    public class Leczna24WebArticle : WebArticleBase
    {
        public Leczna24WebArticle(string uniqueId, NewsProviderBase provider) : base(uniqueId, provider)
        {
            
        }
        class Leczna24WebArticleDownloader : Downloader
        {
            public Leczna24WebArticleDownloader(Uri feedUri) : base(feedUri)
            {
            }

            public override StreamReader OverrideStreamReader(Stream dataStream)
            {
                return new StreamReader(dataStream, new IsoEncoding(), true);
            }
        }

        public override Downloader CreateNewDownloader()
        {
            return new Leczna24WebArticleDownloader(new Uri(this.UniqueId));
        }

        //sorry for not using LINQ but I had some problems with it - probably because of HtmlAgilityPack
        protected override string GetTitle()
        {

            foreach (HtmlNode node in DownloadedHtmlDocument.DocumentNode.Descendants())
            {
                if (node.Attributes.Contains("class") && (node.GetAttributeValue("class", "") == "artykul_tytul"))
                {
                    return node.InnerText;
                }
            }
            return "Unable to download article title";
            //return HtmlDocument.DocumentNode
            //        .Descendants()
            //        .FirstOrDefault(x => x.Attributes["class"].Value == "artykul_tytul").ToString();
        }

        protected override string GetHeadline()
        {
            foreach (HtmlNode node in DownloadedHtmlDocument.DocumentNode.Descendants())
            {
                if (node.Attributes.Contains("class") && (node.GetAttributeValue("class", "") == "artykul_lead"))
                {
                    return node.InnerText;
                }
            }
            return "Unable to download article headline";
        }

        protected override string GetImagePath()
        {
            foreach (HtmlNode node in DownloadedHtmlDocument.DocumentNode.Descendants())
            {
                if (node.Attributes.Contains("class") && (node.GetAttributeValue("class", "") == "noprint informacje_content_img"))
                {
                    string s = node.GetAttributeValue("style", "");
                    s = s.Replace("background-image: url(", "");
                    s = s.Replace(");", "");
                    s = this.Provider.ProviderNamespace + s;
                    return s;
                }
            }
            return "Unable to download article image";
            //    .Where(elements => (string) elements.Attribute("class") == "noprint informacje_content_img")
            //    .Select(elements => elements.Attribute("style").Value).FirstOrDefault();
        }

        protected override string GetArticleBody()
        {            
            foreach (HtmlNode node in DownloadedHtmlDocument.DocumentNode.Descendants())
            {
                if (node.Attributes.Contains("class") && (node.GetAttributeValue("class", "") == "artykul_tresc"))
                {
                    this.IsPrepared = true;
                    return node.InnerHtml;
                }
            }
            return "Unable to download article body";
            //    .Where(elements => (string)elements.Attribute("class") == "artykul_tresc")
            //    .Select(elements => elements.Value).FirstOrDefault();
        }
    }
}
