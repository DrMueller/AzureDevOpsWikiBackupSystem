using System.IO.Abstractions;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.Orchestration.Services;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.Orchestration.Services.Implementation;
using StructureMap;

namespace Mmu.AzureDevOpsWikiBackupSystem.Infrastructure.DependencyInjection
{
    public class ApplicationRegistry : Registry
    {
        public ApplicationRegistry()
        {
            Scan(scanner =>
            {
                scanner.AssemblyContainingType<ApplicationRegistry>();
                scanner.WithDefaultConventions();
            });

            For<IFileSystem>().Use<FileSystem>().Singleton();
            For<IBackupOrchestrationService>().Use<BackupOrchestrationService>().Singleton();
        }
    }
}
