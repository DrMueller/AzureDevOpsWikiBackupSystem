namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.GitRepo.Services.Servants
{
    public interface IRepoPathServant
    {
        void CleanUp(string repoDirectory);
    }
}