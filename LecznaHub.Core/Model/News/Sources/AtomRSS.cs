using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LecznaHub.Core.Model.Common;
using LecznaHub.Core.Providers;

namespace LecznaHub.Core.Model.News.Sources
{
    public static class AtomRSS
    {
        public static async Task<UniversalNewsCollection> GetRssFeedAsync(string feedurl)
        {
            //Download RSS feed
            var request = new Request(feedurl);
            var data = await Downloader.DownloadWebStringAsync(request);

            XDocument newsXmlDocument = XDocument.Parse(data);
            var XmlItems = newsXmlDocument.Descendants("item");
            //Cast items into model
            UniversalNewsCollection collection = new UniversalNewsCollection();
            foreach (XElement item in XmlItems)
            {
                string id = item.Element("link").Value;
                string title = item.Element("title").Value;
                string description = item.Element("description").Value;

                collection.Items.Add(new UniversalNewsItem
                {
                    UniqueId = id,
                    Title = title,
                    Description = description
                });
            }
            return collection;
        }
    }
}
