using System.Threading.Tasks;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.GitRepo.Services;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.Orchestration.Services.Implementation
{
    public class BackupOrchestrationService : IBackupOrchestrationService
    {
        private readonly IGitRepoDownloader _gitRepoDownloader;

        public BackupOrchestrationService(
                    IGitRepoDownloader gitRepoDownloader)
        {
            _gitRepoDownloader = gitRepoDownloader;
        }

        public Task CreateBackupAsync(string baseDirectory)
        {
            _gitRepoDownloader.DownloadRepo(baseDirectory);
            return Task.CompletedTask;
        }
    }
}