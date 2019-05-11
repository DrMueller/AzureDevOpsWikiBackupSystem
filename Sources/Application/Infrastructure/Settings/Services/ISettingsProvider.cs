using Mmu.AzureDevOpsWikiBackupSystem.Infrastructure.Settings.Models;

namespace Mmu.AzureDevOpsWikiBackupSystem.Infrastructure.Settings.Services
{
    public interface ISettingsProvider
    {
        AppSettings ProvideSettings();
    }
}