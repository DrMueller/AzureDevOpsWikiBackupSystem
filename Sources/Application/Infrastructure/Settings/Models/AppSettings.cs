using System;
using Mmu.Mlh.LanguageExtensions.Areas.Invariance;

namespace Mmu.AzureDevOpsWikiBackupSystem.Infrastructure.Settings.Models
{
    public class AppSettings
    {
        public string AzureDevOpsRepoAccessToken { get; }
        public Uri AzureDevOpsRepoPath { get; }

        public AppSettings(string azureDevOpsRepoAccessToken, Uri azureDevOpsRepoPath)
        {
            Guard.StringNotNullOrEmpty(() => azureDevOpsRepoAccessToken);
            Guard.ObjectNotNull(() => azureDevOpsRepoPath);

            AzureDevOpsRepoAccessToken = azureDevOpsRepoAccessToken;
            AzureDevOpsRepoPath = azureDevOpsRepoPath;
        }
    }
}