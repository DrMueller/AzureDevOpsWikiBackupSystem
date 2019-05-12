using System.IO;
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
                DeleteDirectories(fullPath);
            }

            var newDirectory = _fileSystem.Directory.CreateDirectory(fullPath);
            return newDirectory.FullName;
        }

        private void DeleteDirectories(string directory)
        {
            foreach (var subdirectory in _fileSystem.Directory.EnumerateDirectories(directory))
            {
                DeleteDirectories(subdirectory);
            }

            foreach (var fileName in _fileSystem.Directory.EnumerateFiles(directory))
            {
                var fileInfo = _fileSystem.FileInfo.FromFileName(fileName);
                fileInfo.Attributes = FileAttributes.Normal;
                fileInfo.Delete();
            }

            _fileSystem.Directory.Delete(directory);
        }
    }
}