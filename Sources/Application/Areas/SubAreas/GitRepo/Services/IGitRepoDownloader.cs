namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.GitRepo.Services
{
    public interface IGitRepoDownloader
    {
        void DownloadRepo(string baseDirectory);
    }
}
