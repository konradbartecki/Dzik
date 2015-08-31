using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LecznaHub.Core.Providers;
using Newtonsoft.Json;

namespace LecznaHub.Core.Model
{
    public static class Config
    {
        public const string OpenLecznaApiEndpoint = "http://otwartaleczna.azurewebsites.net/api/v1/";
        public const string NewsStoreFolderName = "News";
        public const string NewsDataStoreFilename = "NewsDataStore.json";

        public static class Json
        {
            public static JsonSerializerSettings GetJsonSerializerSettings()
            {
                return new JsonSerializerSettings
                {
                    //TypeNameHandling = TypeNameHandling.Auto
                };
            }
        }

        public static class News
        {
            public static List<NewsProviderBase> NewsProvidersList = new List<NewsProviderBase>
            {
                new Leczna24Old()
            };
        }
    }


}
