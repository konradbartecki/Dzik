using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LecznaHub.Core.Helpers;

namespace LecznaHub.Core.Model.Common
{
    public static class Downloader
    {
        public static async Task<string> DownloadWebStringAsync(Request request)
        {
            WebRequest webrequest = request.AsWebRequest();
            WebResponse response = await webrequest.GetResponseAsync();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = GetCustomStreamReader(dataStream, request);
            string responseFromServer = reader.ReadToEnd();
            reader.Dispose();
            dataStream.Dispose();
            response.Dispose();
            return responseFromServer;
        }

        private static StreamReader GetCustomStreamReader(Stream datastream, Request request)
        {
            switch (request.UsedEncoding)
            {
                case Request.Encoding.Latin2:
                    return new StreamReader(datastream, new IsoEncoding(), true);
                default:
                    return new StreamReader(datastream);
            }
        }
    }

    public class Request
    {
        public Uri Uri { get; set; }
        public Encoding UsedEncoding { get; set; }

        public enum Encoding
        {
            UTF8,
            Latin2
        }

        public Request(Uri uri)
        {
            this.Uri = uri;
            this.UsedEncoding = Encoding.UTF8;
        }
        public Request(string uri) : this(new Uri(uri))
        {
        }

        public Request(Uri uri, Encoding encoding)
        {
            this.Uri = uri;
            this.UsedEncoding = encoding;
        }

        public Request(string uri, Encoding encoding) : this(new Uri(uri), encoding)
        {
        }

        public WebRequest AsWebRequest()
        {
            return WebRequest.Create(this.Uri);
        }
    }
}
