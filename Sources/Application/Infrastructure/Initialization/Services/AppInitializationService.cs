using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.Orchestration.Services;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.Orchestration.Services.Implementation;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.GitRepo.Services;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.GitRepo.Services.Implementation;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.GitRepo.Services.Servants;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.GitRepo.Services.Servants.Implementation;
using Mmu.AzureDevOpsWikiBackupSystem.Infrastructure.Settings.Services;
using Mmu.AzureDevOpsWikiBackupSystem.Infrastructure.Settings.Services.Implementation;
using Mmu.Mlazh.AzureApplicationExtensions.Areas.AzureAppInitialization.Services;
using Mmu.Mlazh.AzureApplicationExtensions.Areas.FunctionContext.Contexts.Services;
using Mmu.Mlh.ServiceProvisioning.Areas.Initialization.Models;
using StructureMap;

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
                logger,
                Initialized);
        }

        private static void Initialized(IContainer obj)
        {
            obj.Configure(cfg =>
            {
                cfg.For<IBackupOrchestrationService>().Use<BackupOrchestrationService>().Singleton();
                cfg.For<IGitRepoDownloader>().Use<GitRepoDownloader>().Singleton();
                cfg.For<ISettingsProvider>().Use<SettingsProvider>().Singleton();
                cfg.For<IRepoPathServant>().Use<RepoPathServant>().Singleton();
            });
        }
    }
}