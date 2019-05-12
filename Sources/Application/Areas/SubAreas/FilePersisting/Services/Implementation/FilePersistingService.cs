using System.IO.Abstractions;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Mmu.AzureDevOpsWikiBackupSystem.Infrastructure.Settings.Services;
using Mmu.Mlazh.AzureApplicationExtensions.Areas.Logging;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.FilePersisting.Services.Implementation
{
    public class FilePersistingService : IFilePersistingService
    {
        private readonly ILoggingService _loggingService;
        private readonly IFileSystem _fileSystem;
        private readonly ISettingsProvider _settingsProvider;

        public FilePersistingService(
            ILoggingService loggingService,
            IFileSystem fileSystem,
            ISettingsProvider settingsProvider)
        {
            _loggingService = loggingService;
            _fileSystem = fileSystem;
            _settingsProvider = settingsProvider;
        }

        public async Task PersistRepoZipAsync(string filePath)
        {
            var connectionString = _settingsProvider.ProvideSettings().StorageConnectionString;

            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var cloudBlobClient = storageAccount.CreateCloudBlobClient();
            var containerRef = cloudBlobClient.GetContainerReference("azuredevopsbackup");
            var fileName = _fileSystem.FileInfo.FromFileName(filePath);

            var cloudBlockBlob = containerRef.GetBlockBlobReference(fileName.Name);

            _loggingService.LogInformation("Uploading..");

            await cloudBlockBlob.UploadFromFileAsync(filePath);
            _loggingService.LogInformation("Upload finished..");
        }
    }
}