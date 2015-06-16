using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using LecznaHub.Android.Controls;

namespace LecznaHub.Android.Views
{
    [Activity(Label = "View for DetailView")]
    public class DetailView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainView);

        }

        public class BindableWebView : WebView
        {
            private string _text;

            public BindableWebView(Context context, IAttributeSet attrs)
                : base(context, attrs)
            {
            }

            public string Text
            {
                get { return _text; }
                set
                {
                    if (string.IsNullOrEmpty(value)) return;

                    _text = value;

                    LoadData(_text, "text/html", "utf-8");
                    UpdatedHtmlContent();
                }
            }

            public event EventHandler HtmlContentChanged;

            private void UpdatedHtmlContent()
            {
                var handler = HtmlContentChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
        }
    }
}