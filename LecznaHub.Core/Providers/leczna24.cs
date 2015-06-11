using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using LecznaHub.Core.Model;

namespace LecznaHub.Core.Providers
{
    public class Leczna24 : NewsProviderBase
    {
        public Leczna24()
        {
            this.NewsFeedUri = new Uri("http://leczna24.pl/rss/informacje_utf8.php");
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

                collection.Items.Add(new Leczna24NewsItem(id, title, description));
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
        public Leczna24NewsItem(string uniqueId, string title, string description)
            : base (uniqueId, title, GetImagePath(description), GetDescription(description), new Leczna24WebArticle(uniqueId))
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

    public class Leczna24WebArticle : WebArticleBase
    {
        public Leczna24WebArticle(string uniqueId) : base(uniqueId)
        {
            
        }
        
        protected override string GetHeadline()
        {
            return
                WebPageXDocument.Elements("h2")
                    .Where(elements => (string)elements.Attribute("class") == "artykul_lead")
                    .Select(elements => elements.Value).FirstOrDefault();
        }

        protected override string GetImagePath()
        {
            return
                WebPageXDocument.Elements("h2")
                    .Where(elements => (string) elements.Attribute("class") == "noprint informacje_content_img")
                    .Select(elements => elements.Attribute("style").Value).FirstOrDefault();
        }

        protected override string PrepareHtmlPage()
        {
            this.IsPrepared = true;
            return
                WebPageXDocument.Elements("h2")
                    .Where(elements => (string)elements.Attribute("class") == "artykul_tresc")
                    .Select(elements => elements.Value).FirstOrDefault();
        }

        protected override string GetTitle()
        {
            return
                WebPageXDocument.Elements("h1")
                    .Where(elements => (string)elements.Attribute("class") == "artykul_tytul")
                    .Select(elements => elements.Value).FirstOrDefault();
        }
    }
}
