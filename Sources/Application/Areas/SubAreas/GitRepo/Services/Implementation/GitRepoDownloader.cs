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
        private readonly IFileSystem _fileSystem;
        private readonly ILoggingService _loggingService;
        private readonly IRepoPathServant _repoPathServant;
        private readonly ISettingsProvider _settingsProvider;

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

        public void CleanUp(string repoDirectory)
        {
            _repoPathServant.CleanUp(repoDirectory);
        }

        public RepoDownloadResult DownloadRepo(string baseDirectory)
        {
            var repoDirectory = _fileSystem.Path.Combine(baseDirectory, "GitRepo");
            _repoPathServant.CleanUp(repoDirectory);
            var newDirectory = _fileSystem.Directory.CreateDirectory(repoDirectory);

            var settings = _settingsProvider.ProvideSettings();

            var options = new CloneOptions
            {
                CredentialsProvider = (url, user, cred) => new UsernamePasswordCredentials
                {
                    Username = settings.AzureDevOpsRepoAccessToken,
                    Password = string.Empty
                }
            };

            _loggingService.LogInformation($"Starting to download Repo to {repoDirectory}..");
            var clonedRepo = Repository.Clone(settings.AzureDevOpsRepoPath.AbsoluteUri, repoDirectory, options);
            _loggingService.LogInformation($"Repo downloaded..");

            var directoryInfo = _fileSystem.DirectoryInfo.FromDirectoryName(clonedRepo).Parent;
            return new RepoDownloadResult(directoryInfo.FullName);
        }
    }
}