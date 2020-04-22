using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace MeerkatUpdater.Core.Test.GeneralUsage
{
    public static class CreatedFoldersManager
    {
        private static readonly Random random = new Random();

        private static Dictionary<string, string> ListOfCreatedFolders { get; set; }

        public static string GenerateNewFolder(string testKey)
        {
            var id = GetRandomNumber(1, 1000);
            var folderName = $"FolderToTest_{DateTime.Now.ToString("yyyyMMdd-HHMMss-fff", CultureInfo.InvariantCulture)}-{id}";
            var folder = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            (ListOfCreatedFolders ?? (ListOfCreatedFolders = new Dictionary<string, string>())).TryAdd(testKey, folder);
            return folder;
        }

        public static void TierDownTest(string testKey)
        {
            if (ListOfCreatedFolders.TryGetValue(testKey, out string path))
                Directory.Delete(path, true);
        }

        private static int GetRandomNumber(int min, int max)
        {
            lock (random)
            {
                return random.Next(min, max);
            }
        }
    }
}