using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Droid.Views;
using LecznaHub.Android.Controls;
using LecznaHub.Core.ViewModel;

namespace LecznaHub.Android.Views
{
    //Todo: make dynamic activity label from vm.item.providername
    [Activity(Label = "£êczna24.pl")]
    public class DetailView : MvxActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DetailView);

            var web_view = FindViewById<WebView>(Resource.Id.webView1);
            //white flash on load hack
            web_view.SetBackgroundColor(Color.Black);
            web_view.Settings.JavaScriptEnabled = true;

            //var nativeWebview = this.FindViewById<WebView>(Resource.Id.webView2);
            //var set = this.CreateBindingSet<DetailView, DetailViewModel>();
            // for non-default properties use 'For':s
            //set.Bind(localWebView).For(wb => wb.Text).To(vm => vm.HtmlText);
            //set.Apply();

        }
    }
}