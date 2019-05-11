using LibGit2Sharp;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.GitRepo.Services.Servants;
using Mmu.AzureDevOpsWikiBackupSystem.Infrastructure.Settings.Services;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.GitRepo.Services.Implementation
{
    public class GitRepoDownloader : IGitRepoDownloader
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly IRepoPathServant _repoPathServant;

        public GitRepoDownloader(ISettingsProvider settingsProvider, IRepoPathServant repoPathServant)
        {
            _settingsProvider = settingsProvider;
            _repoPathServant = repoPathServant;
        }

        public void DownloadRepo(string baseDirectory)
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

            var clonedRepo = Repository.Clone(settings.AzureDevOpsRepoPath.AbsoluteUri, repoPath, options);
        }
    }
}