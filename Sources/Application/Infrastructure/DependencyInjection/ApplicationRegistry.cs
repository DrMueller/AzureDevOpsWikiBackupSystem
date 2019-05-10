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
        }
    }
}
