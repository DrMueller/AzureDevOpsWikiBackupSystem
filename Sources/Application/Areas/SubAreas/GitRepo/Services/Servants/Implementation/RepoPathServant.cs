using System.IO;
using System.IO.Abstractions;
using Mmu.Mlazh.AzureApplicationExtensions.Areas.Logging;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.GitRepo.Services.Servants.Implementation
{
    public class RepoPathServant : IRepoPathServant
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILoggingService _loggingService;

        public RepoPathServant(IFileSystem fileSystem, ILoggingService loggingService)
        {
            _fileSystem = fileSystem;
            _loggingService = loggingService;
        }

        public void CleanUp(string repoDirectory)
        {
            if (_fileSystem.Directory.Exists(repoDirectory))
            {
                _loggingService.LogInformation($"Deleting subdirectories..");
                DeleteDirectories(repoDirectory);
            }
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