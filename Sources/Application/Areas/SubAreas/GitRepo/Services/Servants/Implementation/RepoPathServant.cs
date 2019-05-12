using System.IO;
using System.IO.Abstractions;
using Mmu.Mlh.LanguageExtensions.Areas.Collections;

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

        private static void RemoveReadOnlyAndDelete(FileInfoBase fileInfo)
        {
            if ((fileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                fileInfo.Attributes &= ~FileAttributes.ReadOnly;
            }

            fileInfo.Delete();
        }

        private void DeleteDirectories(string directory)
        {
            var dirInfo = _fileSystem.DirectoryInfo.FromDirectoryName(directory);
            dirInfo.GetFiles("*.pack", SearchOption.AllDirectories).ForEach(f => RemoveReadOnlyAndDelete(f));
            dirInfo.GetFiles("*.idx", SearchOption.AllDirectories).ForEach(f => RemoveReadOnlyAndDelete(f));
            _fileSystem.Directory.Delete(directory, true);
        }
    }
}