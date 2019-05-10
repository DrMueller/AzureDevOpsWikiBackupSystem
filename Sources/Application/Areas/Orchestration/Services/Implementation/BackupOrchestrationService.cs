using System.Threading.Tasks;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.Orchestration.Services.Implementation
{
    public class BackupOrchestrationService : IBackupOrchestrationService
    {
        public BackupOrchestrationService()
        {
        }

        public Task CreateBackupAsync()
        {
            return Task.CompletedTask;
        }
    }
}
