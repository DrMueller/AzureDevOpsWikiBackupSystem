using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.Orchestration.Services;
using Mmu.AzureDevOpsWikiBackupSystem.Infrastructure.Initialization.Services;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.AzureFunctions
{
    public static class Createbackup
    {
        [FunctionName("CreateBackup")]
        [SuppressMessage("Microsoft.Usage", "CA1801", Justification = "Needed by the Framework")]
        public static async Task RunAsync([TimerTrigger("0 0 0 * * MON")]TimerInfo myTimer, ILogger logger, ExecutionContext context)
        {
            await RunInternalAsync(context, logger);
        }

        public static async Task RunInternalAsync(ExecutionContext context, ILogger logger)
        {
            var azureFuncContext = AppInitializationService.Initialize(context, logger);

            await azureFuncContext.ExecuteActionAsync(
                async serviceLocator =>
                {
                    var orchestrator = serviceLocator.GetService<IBackupOrchestrationService>();
                    await orchestrator.CreateBackupAsync(context.FunctionAppDirectory);
                });
        }

        [FunctionName("Test")]
        [SuppressMessage("Microsoft.Usage", "CA1801", Justification = "Needed by the Framework")]
        public static async Task<IActionResult> ExecuteAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger logger, ExecutionContext context)
        {
            await RunInternalAsync(context, logger);
            var ok = new OkResult();
            return ok;
        }
    }
}