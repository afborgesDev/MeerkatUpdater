using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

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

        public static void TierDownTest(string testKey, int retryCount = 0)
        {
            const int MaximumRetryCount = 3;

            if (retryCount > MaximumRetryCount)
                return;

            if (ListOfCreatedFolders.TryGetValue(testKey, out string path))
            {
                try
                {
                    Directory.Delete(path, true);
                }
                catch
                {
                    Thread.Sleep(TimeSpan.FromSeconds(3));
                    retryCount++;
                    TierDownTest(testKey, retryCount);
                }
            }
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