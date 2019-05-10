using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.Orchestration.Services;
using Mmu.AzureDevOpsWikiBackupSystem.Infrastructure.Initialization.Services;
using Mmu.Mlazh.AzureApplicationExtensions.Areas.FunctionContext.Contexts.Services;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.AzureFunctions
{
    public static class Createbackup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Needed by the Framework")]
        [FunctionName("CreateBackup")]
        public static async Task RunAsync([TimerTrigger("0 0 0 * * *")]TimerInfo myTimer, ILogger logger, ExecutionContext context)
        {
            logger.LogInformation("Received timeout..");

            var azureFuncContext = AppInitializationService.Initialize(context, logger);
            await RunInternalAsync(azureFuncContext);
        }

        public static async Task RunInternalAsync(IAzureFunctionContext azureFuncContext)
        {
            await azureFuncContext.ExecuteActionAsync(
                async serviceLocator =>
                {
                    var orchestrator = serviceLocator.GetService<IBackupOrchestrationService>();
                    await orchestrator.CreateBackupAsync();
                });
        }
    }
}
