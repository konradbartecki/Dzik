using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace LecznaHub.Shared.Common
{
    public static class WebViewerHelper
    {
        public static string HtmlHeader(double viewportWidth, double height, string theme, string font) //adapt parametres
        {
            var head = new StringBuilder();
            head.Append("<head>");

            head.Append("<meta name=\"viewport\" content=\"initial-scale=1, maximum-scale=1, user-scalable=0\"/>");
            //head.Append("<script type=\"text/javascript\">" +
            //    "document.documentElement.style.msScrollTranslation = 'vertical-to-horizontal';" +
            //    "</script>"); //horizontal scrolling
            //                  //head.Append("<meta name=\"viewport\" content=\"width=720px\">");
            head.Append("<style>");
            head.Append("html { -ms-text-size-adjust:100%;}");
            head.Append(string.Format("h2{{font-size: 12px}} " +
            "body {{background:{0};color:{1};font-family:'Segoe UI';font-size:10px;margin:0;padding:0;display: block;" +
            "height: 100%;" +
            "max-width: 100%;" +
            "z-index: 0;}}" +
            //"article{{column-fill: auto;column-gap: 80px;column-width: 500px; column-height:100%; height:630px;" +
            "}}" +
            "img,p.object,iframe {{ max-width:100%; height:auto }}", theme, font));
            head.Append(string.Format("a {{color:blue}}"));
            head.Append("</style>");

            // head.Append(NotifyScript);
            head.Append("</head>");
            return head.ToString();
        }
        public static string WrapHtml(string htmlSubString, ApplicationTheme currentTheme , double viewportWidth, double height)
        {


            var html = new StringBuilder();
            html.Append("<html>");

            string theme;
            string font;
            if (currentTheme == ApplicationTheme.Dark)
            {
                theme = "black";
                font = "white";
            }

            else
            {
                theme = "white";
                font = "black";
            }

            html.Append(HtmlHeader(viewportWidth, height, theme, font));
            html.Append("<body><article class=\"content\">");
            html.Append(htmlSubString);
            html.Append("</article></body>");
            html.Append("</html>");
            return html.ToString();
        }
    }
}
