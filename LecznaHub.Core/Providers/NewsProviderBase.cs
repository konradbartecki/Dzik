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
        /// <summary>
        /// This Uri contains address of where will news be downloaded from. Most likely address of RSS
        /// </summary>
        internal Uri NewsFeedUri;

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
