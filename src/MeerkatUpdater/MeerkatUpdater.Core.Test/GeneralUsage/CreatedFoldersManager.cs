using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;

namespace MeerkatUpdater.Core.Test.GeneralUsage
{
    public static class CreatedFoldersManager
    {
        private static ConcurrentDictionary<string, string> ListOfCreatedFolders { get; set; }

        public static string GenerateNewFolder(string testKey)
        {
            var folderName = $"FolderToTest_{DateTime.Now.ToString("yyyyMMdd-HHMMSS.zzz", CultureInfo.InvariantCulture)}";
            var folder = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            (ListOfCreatedFolders ?? (ListOfCreatedFolders = new ConcurrentDictionary<string, string>())).TryAdd(testKey, folder);
            return folder;
        }
    }
}