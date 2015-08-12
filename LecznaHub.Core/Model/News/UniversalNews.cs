using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LecznaHub.Core.Helpers;
using PCLStorage;

namespace LecznaHub.Core.Model.News
{
    /// <summary>
    /// Provider independent fully downloaded universal news item that can be stored locally
    /// </summary>
    public class UniversalNewsItem
    {
        //Work in progress
        /// <summary>
        /// HTTP Link to the full article
        /// </summary>
        public string UniqueId { get; set; }
        public string Title { get; set; }
        public string ProviderName { get; set; }
        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string DownloadedArticleHtml { get; set; }
        public Uri WebsiteArticleUri => new Uri(this.UniqueId);

        //private string _filename;
        private bool _isImageCached;

        public override string ToString()
        {
            return Title;
        }

        public UniversalNewsItem()
        {
            
        }

        public UniversalNewsItem(NewsItemBase newsBase)
        {
            UniqueId = newsBase.UniqueId;
            Title = newsBase.Title;
            Description = newsBase.Description;
            ProviderName = newsBase.NewsProvider.Name;
            //TODO: Check formatted html article theme against current theme
            DownloadedArticleHtml = newsBase.WebArticle.FormattedHtmlDocument;
            ImagePath = newsBase.WebArticle.ImagePath;
        }

        public async Task CacheImageAsync()
        {
            if (_isImageCached) return;
            //prepare folder
            var folder = await StorageHelper.GetNewsFolderAsync();
            //download image
            var imagebytes = await BytesDownloader.DownloadBytesAsync(ImagePath);

            var extension = ImagePath.Substring(ImagePath.Length - 4);
            var filename = String.Format("{0}{1}", Guid.NewGuid(), extension);

            var file = await folder.CreateFileAsync(filename, CreationCollisionOption.GenerateUniqueName);          
            
            var stream = await file.OpenAsync(FileAccess.ReadAndWrite);
            await stream.WriteAsync(imagebytes, 0, imagebytes.Length);
            await stream.FlushAsync();
            ImagePath = file.Path;
            _isImageCached = true;
        }
    }

    public class UniversalNewsCollection
    {
        public string ProviderName { get; set; }
        public List<UniversalNewsItem> Items { get; set; }

        public UniversalNewsCollection(string name)
        {
            this.ProviderName = name;
            this.Items = new List<UniversalNewsItem>();
        }

        public UniversalNewsCollection()
        {
            this.Items = new List<UniversalNewsItem>();
        }
    }

    public class UniversalNewsItemStore
    {
        public string Version { get; set; }

        public List<UniversalNewsCollection> NewsCollections { get; set; }

        public UniversalNewsItemStore()
        {
            this.Version = AssemblyVersionHelper.GetAssemblyVersion(this);
            this.NewsCollections = new List<UniversalNewsCollection>();
            //create news store folder
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            rootFolder.CreateFolderAsync(Config.NewsStoreFolderName, CreationCollisionOption.OpenIfExists).Wait();
        }

    }
}
