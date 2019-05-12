using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Mmu.AzureDevOpsWikiBackupSystem.Infrastructure.Settings.Services;
using Mmu.Mlazh.AzureApplicationExtensions.Areas.Logging;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.FilePersisting.Services.Implementation
{
    public class PersistedFilesCleanupService : IPersistedFilesCleanupService
    {
        private readonly ILoggingService _loggingService;
        private readonly ISettingsProvider _settingsProvider;

        public PersistedFilesCleanupService(
            ILoggingService loggingService,
            ISettingsProvider settingsProvider)
        {
            _loggingService = loggingService;
            _settingsProvider = settingsProvider;
        }

        public async Task CleanUpOldRepoZipsAsync()
        {
            var connectionString = _settingsProvider.ProvideSettings().StorageConnectionString;

            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var cloudBlobClient = storageAccount.CreateCloudBlobClient();
            var containerRef = cloudBlobClient.GetContainerReference(Constants.StorageBlobContainerName);

            _loggingService.LogInformation("Loading existing blobs..");
            var blobs = await containerRef.ListBlobsSegmentedAsync(null);

            _loggingService.LogInformation("Blobs loaded..");
            var deleteCheckTasks = blobs.Results.Select(blobItem => CheckDeleteBlobEntryAsync(blobItem, containerRef));
            await Task.WhenAll(deleteCheckTasks);
        }

        private static double ParseDaysSinceUploaded(string fileName)
        {
            var fileNameWithoutExtension = fileName.Replace(".zip", string.Empty, StringComparison.OrdinalIgnoreCase);
            fileNameWithoutExtension = fileNameWithoutExtension.Substring(fileNameWithoutExtension.IndexOf("_", StringComparison.OrdinalIgnoreCase) + 1);
            var uploadDate = DateTime.ParseExact(fileNameWithoutExtension, "yyyyMMdd", CultureInfo.InvariantCulture);
            var diffDays = (DateTime.UtcNow.Date - uploadDate).TotalDays;
            return diffDays;
        }

        private async Task CheckDeleteBlobEntryAsync(IListBlobItem blobItem, CloudBlobContainer containerRef)
        {
            var blobFileName = blobItem.Uri.Segments.Last();

            _loggingService.LogInformation($"Checking {blobFileName}..");
            var diffDays = ParseDaysSinceUploaded(blobFileName);
            _loggingService.LogInformation($"File {blobFileName} was saved {diffDays} ago..");

            if (diffDays >= 7)
            {
                _loggingService.LogInformation($"Deleting File {blobFileName}..");
                var blockBlob = containerRef.GetBlockBlobReference(blobFileName);
                await blockBlob.DeleteAsync();
                _loggingService.LogInformation($"File {blobFileName} deleted..");
            }
        }
    }
}