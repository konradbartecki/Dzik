using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LecznaHub.Core.Model
{
    public class Downloader
    {
        public Downloader(Uri feedUri)
        {
            request = WebRequest.Create(feedUri);
        }

        private WebRequest request { get; set; }

        public async Task<string> GetNewsAsync()
        {
            WebResponse response = await request.GetResponseAsync();
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Cleanup the streams and the response.
            reader.Dispose();
            dataStream.Dispose();
            response.Dispose();

            return responseFromServer;

        }
    }
}
