using System;
using Mmu.Mlh.LanguageExtensions.Areas.Invariance;

namespace Mmu.AzureDevOpsWikiBackupSystem.Infrastructure.Settings.Models
{
    public class AppSettings
    {
        public string AzureDevOpsRepoAccessToken { get; }
        public Uri AzureDevOpsRepoPath { get; }
        public string StorageConnectionString { get; }

        public AppSettings(string azureDevOpsRepoAccessToken, Uri azureDevOpsRepoPath, string storageConnectionString)
        {
            Guard.StringNotNullOrEmpty(() => azureDevOpsRepoAccessToken);
            Guard.ObjectNotNull(() => azureDevOpsRepoPath);
            Guard.StringNotNullOrEmpty(() => storageConnectionString);

            AzureDevOpsRepoAccessToken = azureDevOpsRepoAccessToken;
            AzureDevOpsRepoPath = azureDevOpsRepoPath;
            StorageConnectionString = storageConnectionString;
        }
    }
}