using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LecznaHub.Core.Helpers;

namespace LecznaHub.Core.Model
{
    public class Downloader
    {
        public Downloader(Uri feedUri)
        {
            Request = WebRequest.Create(feedUri);
        }

        public Downloader(Uri feedUri, string requestContentType, string requestMethod)
        {
            Request = WebRequest.Create(feedUri);
            Request.ContentType = requestContentType;
            Request.Method = requestMethod;
        }

        private WebRequest Request { get; }

        public async Task<string> GetPageAsync()
        {
            WebResponse response = await Request.GetResponseAsync();
            //Request.ContentType = "application/json";
            //Request.Method = "GET";
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            // Create new reader through method below to allow other classes to override stream reader creation and specify own encoding
            StreamReader reader = OverrideStreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Cleanup the streams and the response.
            reader.Dispose();
            dataStream.Dispose();
            response.Dispose();

            return responseFromServer;

        }

        /// <summary>
        /// Override this class if you need to create new StreamReader with custom encoding
        /// </summary>
        /// <param name="dataStream"></param>
        /// <returns>DataReader with dataStream and custom encoding</returns>
        public virtual StreamReader OverrideStreamReader(Stream dataStream)
        {
            return new StreamReader(dataStream);
        }
    }

    public static class BytesDownloader
    {

        public static byte[] DownloadBytes(string url)
        {
            WebRequest request = WebRequest.Create(new Uri(url));
            request.GetResponseAsync().RunSynchronously();
            using (WebResponse respone = request.GetResponseAsync().Result)
            using (Stream dataStream = respone.GetResponseStream())
            using (var memoryStream = new MemoryStream())
            {
                dataStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
