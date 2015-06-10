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
    public static class Leczna24
    {
        private static readonly Uri uri = new Uri("http://leczna24.pl/rss/informacje_utf8.php");

        public static async Task<NewsGroups> GetNewsAsync()
        {
            //Download news as string
            Downloader downloader = new Downloader(uri);
            string news = await downloader.GetNewsAsync();

            return GetNewsFromText(news);
        }
        
        //Read as XML and select all items
        public static NewsGroups GetNewsFromText(string news)
        {
            XDocument newsXmlDocument = XDocument.Parse(news);
            var XmlItems = newsXmlDocument.Descendants("item");
            //Cast items into model
            NewsGroups group = new NewsGroups("Leczna24 news");
            foreach (XElement item in XmlItems)
            {
                string id = item.Element("link").Value;
                string title = item.Element("title").Value;
                string description = item.Element("description").Value;

                group.Items.Add(new Leczna24NewsItem(id, title, description));
            }
            return group;
        }

    }



    public class Leczna24NewsItem : NewsItem
    {
        /// <summary>
        /// Łęczna24 news class
        /// </summary>
        /// <param name="uniqueId">News ID and/or link</param>
        /// <param name="title">News title</param>
        /// <param name="imagePath">Not used because img path is contained in description</param>
        /// <param name="description">Contains longer description about news</param>
        public Leczna24NewsItem(string uniqueId, string title, string description)
            : base (uniqueId, title, GetImagePath(description), GetDescription(description))
        {

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
}
