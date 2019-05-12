using System.IO.Abstractions;
using System.Threading.Tasks;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.FilePersisting.Services;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.GitRepo.Services;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.Zipping.Services;
using Mmu.Mlazh.AzureApplicationExtensions.Areas.Logging;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.Orchestration.Services.Implementation
{
    public class BackupOrchestrationService : IBackupOrchestrationService
    {
        private readonly IFilePersistingService _filePersistingService;
        private readonly IFileSystem _fileSystem;
        private readonly IGitRepoDownloader _gitRepoDownloader;
        private readonly ILoggingService _loggingService;
        private readonly IPersistedFilesCleanupService _persistFilesCleanUpService;
        private readonly IZippingService _zippingService;

        public BackupOrchestrationService(
            IFileSystem fileSystem,
            ILoggingService loggingService,
            IGitRepoDownloader gitRepoDownloader,
            IZippingService zippingService,
            IFilePersistingService filePersistingService,
            IPersistedFilesCleanupService persistFilesCleanUpService)
        {
            _fileSystem = fileSystem;
            _loggingService = loggingService;
            _gitRepoDownloader = gitRepoDownloader;
            _zippingService = zippingService;
            _filePersistingService = filePersistingService;
            _persistFilesCleanUpService = persistFilesCleanUpService;
        }

        public async Task CreateBackupAsync(string baseDirectory)
        {
            baseDirectory = @"C:\Tmp";

            _loggingService.LogInformation("Starting backup..");
            var downloadRepoResult = _gitRepoDownloader.DownloadRepo(baseDirectory);
            var zippingResult = _zippingService.ZipDirectory(downloadRepoResult.DirectoryPath);
            await _filePersistingService.PersistRepoZipAsync(zippingResult.ZipFilePath);
            await _persistFilesCleanUpService.CleanUpOldRepoZipsAsync();

            _loggingService.LogInformation("Starting to clean up..");
            _fileSystem.File.Delete(zippingResult.ZipFilePath);
            _gitRepoDownloader.CleanUp(downloadRepoResult.DirectoryPath);
            _loggingService.LogInformation("Backup finished.");
        }
    }
}