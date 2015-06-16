using System;
using Android.Content;
using Android.Util;
using Android.Webkit;

namespace LecznaHub.Android.Controls
{
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