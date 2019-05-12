using System.Threading.Tasks;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.FilePersisting.Services
{
    public interface IFilePersistingService
    {
        Task PersistRepoZipAsync(string filePath);
    }
}