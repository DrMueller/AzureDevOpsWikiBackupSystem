using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.Orchestration.Services;
using Mmu.AzureDevOpsWikiBackupSystem.Infrastructure.Initialization.Services;
using Mmu.Mlazh.AzureApplicationExtensions.Areas.FunctionContext.Contexts.Services;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.AzureFunctions
{
    public static class Createbackup
    {
        [FunctionName("CreateBackup")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801", Justification = "Needed by the Framework")]
        public static async Task RunAsync([TimerTrigger("0 0 0 * * *")]TimerInfo myTimer, ILogger logger, ExecutionContext context)
        {
            var azureFuncContext = AppInitializationService.Initialize(context, logger);
            await RunInternalAsync(azureFuncContext, context.FunctionAppDirectory);
        }

        public static async Task RunInternalAsync(IAzureFunctionContext azureFuncContext, string baseDirectory)
        {
            await azureFuncContext.ExecuteActionAsync(
                async serviceLocator =>
                {
                    var orchestrator = serviceLocator.GetService<IBackupOrchestrationService>();
                    await orchestrator.CreateBackupAsync(baseDirectory);
                });
        }

        [FunctionName("Test")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801", Justification = "Needed by the Framework")]
        public static async Task<IActionResult> ExecuteAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger logger, ExecutionContext context)
        {
            var azureFuncContext = AppInitializationService.Initialize(context, logger);
            await RunInternalAsync(azureFuncContext, context.FunctionAppDirectory);

            var ok = new OkResult();
            return ok;
        }
    }
}