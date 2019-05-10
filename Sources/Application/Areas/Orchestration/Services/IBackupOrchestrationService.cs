using System.Threading.Tasks;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.Orchestration.Services
{
    public interface IBackupOrchestrationService
    {
        Task CreateBackupAsync();
    }
}
