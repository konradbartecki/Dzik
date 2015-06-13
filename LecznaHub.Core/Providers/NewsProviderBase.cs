using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LecznaHub.Core.Model;

namespace LecznaHub.Core.Providers
{
    /// <summary>
    /// This is an implementation of news provider. It can be derived from and provide custom data manipulation for each provider.
    /// All implementations should specify own 
    /// </summary>
    public abstract class NewsProviderBase
    {
        protected NewsProviderBase(string name, string providerNamespace, Uri newsFeedUri)
        {
            this.Name = name;
            this.NewsFeedUri = newsFeedUri;
            this.ProviderNamespace = providerNamespace;
        }

        public string Name { get; private set; }
        /// <summary>
        /// Root of the website: ex. "http://leczna24.pl"
        /// </summary>
        public string ProviderNamespace { get; private set; }
        /// <summary>
        /// This Uri contains address of where will news be downloaded from. Most likely address of RSS
        /// </summary>
        public Uri NewsFeedUri { get; private set; }


        //internal Uri NewsFeedUri;

        //public string ProviderNamespace;

        public virtual async Task<NewsCollection> GetNewsAsync()
        {
            //Download news as string
            Downloader downloader = new Downloader(NewsFeedUri);
            string news = await downloader.GetPageAsync();

            return GetNewsFromDownloadedData(news);
        }

        /// <summary>
        /// Provides custom implementation of selecting items from downloaded data.
        /// Should cast items into NewsItemsBase
        /// </summary>
        /// <param name="data">Data or page received by HTTP</param>
        /// <returns>Collection of NewsItemBase</returns>
        public abstract NewsCollection GetNewsFromDownloadedData(string data);
    }
}
