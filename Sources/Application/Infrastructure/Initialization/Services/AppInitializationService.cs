using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Mmu.Mlazh.AzureApplicationExtensions.Areas.AzureAppInitialization.Services;
using Mmu.Mlazh.AzureApplicationExtensions.Areas.FunctionContext.Contexts.Services;
using Mmu.Mlh.ServiceProvisioning.Areas.Initialization.Models;

namespace Mmu.AzureDevOpsWikiBackupSystem.Infrastructure.Initialization.Services
{
    internal static class AppInitializationService
    {
        public static IAzureFunctionContext Initialize(ExecutionContext executionContext, ILogger logger)
        {
            var containerConfig = ContainerConfiguration.CreateFromAssembly(typeof(AppInitializationService).Assembly);

            return InitializationService.Initialize(
                containerConfig,
                executionContext,
                logger);
        }
    }
}
