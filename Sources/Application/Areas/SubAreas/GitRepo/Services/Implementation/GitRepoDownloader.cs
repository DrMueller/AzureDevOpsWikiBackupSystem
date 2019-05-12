using System.IO.Abstractions;
using LibGit2Sharp;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.GitRepo.Models;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.GitRepo.Services.Servants;
using Mmu.AzureDevOpsWikiBackupSystem.Infrastructure.Settings.Services;
using Mmu.Mlazh.AzureApplicationExtensions.Areas.Logging;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.GitRepo.Services.Implementation
{
    public class GitRepoDownloader : IGitRepoDownloader
    {
        private readonly ILoggingService _loggingService;
        private readonly ISettingsProvider _settingsProvider;
        private readonly IRepoPathServant _repoPathServant;
        private readonly IFileSystem _fileSystem;

        public GitRepoDownloader(
            ILoggingService loggingService,
            ISettingsProvider settingsProvider,
            IRepoPathServant repoPathServant,
            IFileSystem fileSystem)
        {
            _loggingService = loggingService;
            _settingsProvider = settingsProvider;
            _repoPathServant = repoPathServant;
            _fileSystem = fileSystem;
        }

        public RepoDownloadResult DownloadRepo(string baseDirectory)
        {
            var repoPath = _repoPathServant.InitializeDownloadPath(baseDirectory);
            var settings = _settingsProvider.ProvideSettings();

            var options = new CloneOptions
            {
                CredentialsProvider = (url, user, cred) => new UsernamePasswordCredentials
                {
                    Username = settings.AzureDevOpsRepoAccessToken,
                    Password = string.Empty
                }
            };

            _loggingService.LogInformation($"Starting to download Repo to {repoPath}..");
            var clonedRepo = Repository.Clone(settings.AzureDevOpsRepoPath.AbsoluteUri, repoPath, options);
            _loggingService.LogInformation($"Repo downloaded..");

            var directoryInfo = _fileSystem.DirectoryInfo.FromDirectoryName(clonedRepo).Parent;
            return new RepoDownloadResult(directoryInfo.FullName);
        }
    }
}