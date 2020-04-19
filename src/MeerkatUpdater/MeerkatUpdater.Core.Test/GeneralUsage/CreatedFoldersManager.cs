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
            var random = new Random();
            var id = random.Next(1, 1000);
            var folderName = $"FolderToTest_{DateTime.Now.ToString("yyyyMMdd-HHMMss", CultureInfo.InvariantCulture)}-{id}";
            var folder = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            (ListOfCreatedFolders ?? (ListOfCreatedFolders = new ConcurrentDictionary<string, string>())).TryAdd(testKey, folder);
            return folder;
        }

        public static void TierDownTest(string testKey)
        {
            if (ListOfCreatedFolders.TryGetValue(testKey, out string path))
                Directory.Delete(path, true);
        }
    }
}