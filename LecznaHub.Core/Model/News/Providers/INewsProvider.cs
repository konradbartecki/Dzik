using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LecznaHub.Core.Annotations;

namespace LecznaHub.Core.Model.News.Providers
{
    public abstract class NewsProvider
    {
        public string ProviderName { get; set; }
        public abstract Task<UniversalNewsCollection> GetRssAsync();
        public abstract Task<UniversalNewsItem> GetArticleAsync(string uri);
    }
}
