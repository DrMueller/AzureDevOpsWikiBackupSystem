using System.Threading.Tasks;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.FilePersisting.Services
{
    public interface IPersistedFilesCleanupService
    {
        Task CleanUpOldRepoZipsAsync();
    }
}