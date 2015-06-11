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
            request = WebRequest.Create(feedUri);
        }

        private WebRequest request { get; set; }

        public async Task<string> GetPageAsync()
        {
            WebResponse response = await request.GetResponseAsync();
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
}
