using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LecznaHub.Core.Model;
using PCLStorage;

namespace LecznaHub.Core.Helpers
{
    public static class StorageHelper
    {
        public static async Task<IFolder> GetNewsFolderAsync()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            var folderExist = await rootFolder.CheckExistsAsync(Config.NewsStoreFolderName);
            if (folderExist == ExistenceCheckResult.NotFound)
                return null;
            IFolder folder = await rootFolder.GetFolderAsync(Config.NewsStoreFolderName);
            return folder;
        }

        public static async Task<string> GetNewsImagePathAsync(string filename)
        {
            var folder = await GetNewsFolderAsync();
            var doExist = await folder.CheckExistsAsync(filename);
            if (doExist == ExistenceCheckResult.NotFound)
                return null;
            var file = await folder.GetFileAsync(filename);
            return file.Path;
        } 
    }
}
