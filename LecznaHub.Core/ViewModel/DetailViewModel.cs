using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using LecznaHub.Core.Helpers;
using LecznaHub.Core.Model;
using LecznaHub.Shared.Common;

namespace LecznaHub.Core.ViewModel
{
    public class DetailViewModel : MvxViewModel
    {
        //public DetailViewModel(string uniqueId)
        //{
        //    Init(new MainViewModel.DetailParameter() {Id = uniqueId});
        //}

        public void Init(MainViewModel.DetailParameter parameter)
        {
            if (string.IsNullOrEmpty(parameter.Id))
                return;
            try
            {
                //Item = MainViewModel.GetItemAsync(parameter.Id).Result;
                this.Item = AsyncHelpers.RunSync<NewsItemBase>(() => MainViewModel.GetItemAsync(parameter.Id));
                this.HtmlText = WebViewerHelper.WrapHtml(Item.WebArticle.ToString(), "black");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while initializing DetailViewModel");
            }

        }

        public NewsItemBase Item { get; set; }

        public string HtmlText
        {
            get;
            set;
        }
        
    }
}