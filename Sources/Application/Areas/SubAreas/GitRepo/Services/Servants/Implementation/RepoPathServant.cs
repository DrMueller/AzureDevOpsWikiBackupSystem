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

        public string InitializeDownloadPath(string baseDirectory)
        {
            var fullPath = _fileSystem.Path.Combine(baseDirectory, "GitRepo");

            _loggingService.LogInformation($"Initializing {fullPath}..");

            if (_fileSystem.Directory.Exists(fullPath))
            {
                _loggingService.LogInformation($"Deleting subdirectories..");
                DeleteDirectories(fullPath);
            }

            _loggingService.LogInformation($"Creating directory..");
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