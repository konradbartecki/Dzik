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
    [DataContract]
    public class Leczna24Old : NewsProviderBase
    {
        public Leczna24Old() : base ("Łęczna24", "http://leczna24.pl", new Uri("http://leczna24.pl/rss/informacje_utf8.php"))
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

    [DataContract]
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
        class Leczna24WebArticleOldDownloader : OldDownloader
        {
            public Leczna24WebArticleOldDownloader(Uri feedUri) : base(feedUri)
            {
            }

            public override StreamReader OverrideStreamReader(Stream dataStream)
            {
                return new StreamReader(dataStream, new IsoEncoding(), true);
            }
        }

        public override OldDownloader CreateNewDownloader()
        {
            return new Leczna24WebArticleOldDownloader(new Uri(this.UniqueId));
        }

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
        }

        protected override string GetArticleBody()
        {            
            foreach (HtmlNode node in DownloadedHtmlDocument.DocumentNode.Descendants())
            {
                if (node.Attributes.Contains("class") && (node.GetAttributeValue("class", "") == "artykul_tresc"))
                {
                    this.IsPrepared = true;
                    return PrepareImgElements(node);
                }
            }
            return "Unable to download article body";
        }

        private string PrepareImgElements(HtmlNode ArticleBody)
        {
            foreach (var node in ArticleBody.Descendants())
            {
                if (node.Attributes.Contains("src"))
                {
                    node.Attributes["src"].Value = this.Provider.ProviderNamespace + node.Attributes["src"].Value;
                    if(node.Attributes.Contains("style"))
                    {
                        node.Attributes["style"].Remove();
                    }
                }
            }

            return ArticleBody.InnerHtml;
        }
    }
}
