using System;
using Mmu.AzureDevOpsWikiBackupSystem.Infrastructure.Settings.Dto;
using Mmu.AzureDevOpsWikiBackupSystem.Infrastructure.Settings.Models;
using Mmu.Mlazh.AzureApplicationExtensions.Areas.AzureAppSettingsProvisioning.Services;

namespace Mmu.AzureDevOpsWikiBackupSystem.Infrastructure.Settings.Services.Implementation
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly IAzureAppSettingsProvider<AppSettingsDto> _iAzureAppSettingsProvider;

        public SettingsProvider(IAzureAppSettingsProvider<AppSettingsDto> iAzureAppSettingsProvider)
        {
            _iAzureAppSettingsProvider = iAzureAppSettingsProvider;
        }

        public AppSettings ProvideSettings()
        {
            var settingsDto = _iAzureAppSettingsProvider.ProvideSettings();

            return new AppSettings(
                settingsDto.AzureDevOpsRepoAccessToken,
                new Uri(settingsDto.AzureDevOpsRepoPath));
        }
    }
}