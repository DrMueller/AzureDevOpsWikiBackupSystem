using System.IO.Abstractions;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.GitRepo.Services.Servants.Implementation
{
    public class RepoPathServant : IRepoPathServant
    {
        private readonly IFileSystem _fileSystem;

        public RepoPathServant(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public string InitializeDownloadPath(string baseDirectory)
        {
            var fullPath = _fileSystem.Path.Combine(baseDirectory, "GitRepo");
            if (_fileSystem.Directory.Exists(fullPath))
            {
                _fileSystem.Directory.Delete(fullPath, true);
            }

            var newDirectory = _fileSystem.Directory.CreateDirectory(fullPath);
            return newDirectory.FullName;
        }
    }
}