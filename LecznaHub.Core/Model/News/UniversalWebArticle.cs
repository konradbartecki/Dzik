using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LecznaHub.Core.Model.News
{
    public class UniversalWebArticle
    {
        private string _rawHtml;

        public string UniqueId { get; set; }
        public string Title { get; set; }
        public string Headline { get; set; }
        public string ImagePath { get; set; }
       // public string ArticleBody { get; set; }
       // public TYPE Type { get; set; }
    }
}
