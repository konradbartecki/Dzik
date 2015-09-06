using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using LecznaHub.Core.Annotations;
using LecznaHub.Core.Model.News.Sources;

namespace LecznaHub.Core.Model.News.Providers
{
    public class Leczna24 : NewsProvider
    {
        public Leczna24()
        {
            this.ProviderName = "Łęczna24.pl";
        }

        private const string endpoint_utf8 = "http://leczna24.pl/rss/informacje_utf8.php";
        private const string endpoint = "http://leczna24.pl/rss/informacje.php";
        private const string providerNamespace = "http://leczna24.pl/";
        public override async Task<UniversalNewsCollection> GetRssAsync()
        {
            return await AtomRSS.GetRssFeedAsync(endpoint_utf8);
        }

        public override async Task<UniversalNewsItem> GetArticleAsync(string uri)
        {

        }

        private async Task DownloadWebArticles(UniversalNewsCollection collection)
        {
            foreach (UniversalNewsItem item in collection.Items)
            {
                item.
            }
        }

        private UniversalWebArticle GetWebArticleFromRawHtml(string rawhtml)
        {
            UniversalWebArticle webArticle = new UniversalWebArticle();

            HtmlDocument DownloadedHtmlDocument = new HtmlDocument();
            DownloadedHtmlDocument.LoadHtml(rawhtml);

            foreach (HtmlNode node in DownloadedHtmlDocument.DocumentNode.Descendants())
            {
                //Get title
                if (node.Attributes.Contains("class") && (node.GetAttributeValue("class", "") == "artykul_tytul"))
                {
                    webArticle.Title = node.InnerText;
                }
                //Get headline
                if (node.Attributes.Contains("class") && (node.GetAttributeValue("class", "") == "artykul_lead"))
                {
                    webArticle.Headline = node.InnerText;
                }
                //Get ImagePath
                if (node.Attributes.Contains("class") && (node.GetAttributeValue("class", "") == "noprint informacje_content_img"))
                {
                    string s = node.GetAttributeValue("style", "");
                    s = s.Replace("background-image: url(", "");
                    s = s.Replace(");", "");
                    s = providerNamespace + s;
                    webArticle.ImagePath = s;
                }
                //Prepare img elements
                if (node.Attributes.Contains("src"))
                {
                    node.Attributes["src"].Value = providerNamespace + node.Attributes["src"].Value;
                    if (node.Attributes.Contains("style"))
                    {
                        node.Attributes["style"].Remove();
                    }
                }
            }

            return webArticle;
        }
    }
}
