using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LecznaHub.Shared.Common
{
    public static class WebViewerHelper
    {
        public static string HtmlHeader(string theme, string font) //adapt parametres
        {
            var head = new StringBuilder();
            head.Append("<head>");

            head.Append("<meta name=\"viewport\" content=\"initial-scale=1, maximum-scale=1, user-scalable=0\"/>");
            //head.Append("<script type=\"text/javascript\">" +
            //    "document.documentElement.style.msScrollTranslation = 'vertical-to-horizontal';" +
            //    "</script>"); //horizontal scrolling
            //                  //head.Append("<meta name=\"viewport\" content=\"width=720px\">");
            head.Append("<style>");
            head.Append("html { -ms-text-size-adjust: 100% }" );
            head.Append("body {");
            head.AppendFormat("background-color:{0};", theme);
            head.AppendFormat("color:{0};", font);
            head.Append("font-family:'Segoe UI';" +
                        "font-size:14px;" +
                        "margin:0px;" +
                        "padding:0;" +
                        "display: block;" +
                        "height: 100%;" +
                        "width: 100%;" +
                        "z-index: 0;" +
                        "}");
            head.Append("h1 {" +
                        "font-size: 18px;" +
                        "}");
            head.Append("h2 {" +
                        "font-size: 16px;" +
                        "}");
            head.Append("img{" +
                        "width:100%;" +
                        "}");
            head.Append("a{" +
                        "color: #008B8B;" +
                        "mix-blend-mode: difference;" +
                        "}");
        
            //head.Append(string.Format("a {{color:blue}}"));
            head.Append("</style>");

            // head.Append(NotifyScript);
            head.Append("</head>");
            return head.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlSubString">"Article html to be embeeded into styled web page</param>
        /// <param name="requestedBackgroundColor">"black" or "white"</param>
        /// 
        /// 
        /// <returns></returns>
        public static string WrapHtml(string htmlSubString, string requestedBackgroundColor)
        {
            var html = new StringBuilder();
            html.Append("<html>");

            string theme;
            string font;
            if (requestedBackgroundColor == "black")
            {
                theme = "black";
                font = "white";
            }

            else
            {
                theme = "white";
                font = "black";
            }

            html.Append(HtmlHeader(theme, font));
            html.Append("<body><article class=\"content\" style=\"padding-bottom: 5px;\">");
            html.Append(htmlSubString);
            html.Append("</article></body>");
            html.Append("</html>");
            return html.ToString();
        }
    }
}
