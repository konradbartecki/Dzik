using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using LecznaHub.Core.Model;
using LecznaHub.Shared.Common;

namespace LecznaHub.Core.ViewModel
{
    public class DetailViewModel : MvxViewModel
    {
        DetailViewModel(NewsItemBase item)
        {
            Init(item.UniqueId);
        }

        void Init(string uniqueId)
        {
            try
            {
                Item = MainViewModel.GetItemAsync(uniqueId).Result;
                HtmlText = WebViewerHelper.WrapHtml(Item.WebArticle.ToString(), "black");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while initializing DetailViewModel");
            }

        }

        public NewsItemBase Item { get; set; }
        public string HtmlText { get; set; }

    }
}