using System.Threading.Tasks;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.FilePersisting.Services;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.GitRepo.Services;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.Zipping.Services;
using Mmu.Mlazh.AzureApplicationExtensions.Areas.Logging;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.Orchestration.Services.Implementation
{
    public class BackupOrchestrationService : IBackupOrchestrationService
    {
        private readonly ILoggingService _loggingService;
        private readonly IGitRepoDownloader _gitRepoDownloader;
        private readonly IZippingService _zippingService;
        private readonly IFilePersistingService _filePersistingService;
        private readonly IPersistedFilesCleanupService _persistFilesCleanUpService;

        public BackupOrchestrationService(
            ILoggingService loggingService,
            IGitRepoDownloader gitRepoDownloader,
            IZippingService zippingService,
            IFilePersistingService filePersistingService,
            IPersistedFilesCleanupService persistFilesCleanUpService)
        {
            _loggingService = loggingService;
            _gitRepoDownloader = gitRepoDownloader;
            _zippingService = zippingService;
            _filePersistingService = filePersistingService;
            _persistFilesCleanUpService = persistFilesCleanUpService;
        }

        public async Task CreateBackupAsync(string baseDirectory)
        {
            _loggingService.LogInformation("Starting backup..");
            var downloadRepoResult = _gitRepoDownloader.DownloadRepo(baseDirectory);
            var zippingResult = _zippingService.ZipDirectory(downloadRepoResult.DirectoryPath);
            await _filePersistingService.PersistRepoZipAsync(zippingResult.ZipFilePath);
            await _persistFilesCleanUpService.CleanUpOldRepoZipsAsync();
            _loggingService.LogInformation("Backup finished.");
        }
    }
}