using Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.GitRepo.Models;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.GitRepo.Services
{
    public interface IGitRepoDownloader
    {
        RepoDownloadResult DownloadRepo(string baseDirectory);

        void CleanUp(string repoDirectory);
    }
}