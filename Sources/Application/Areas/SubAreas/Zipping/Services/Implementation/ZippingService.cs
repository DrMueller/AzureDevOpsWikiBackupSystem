using System;
using System.IO.Abstractions;
using System.IO.Compression;
using Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.Zipping.Models;
using Mmu.Mlazh.AzureApplicationExtensions.Areas.Logging;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.Zipping.Services.Implementation
{
    public class ZippingService : IZippingService
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILoggingService _loggingService;

        public ZippingService(
            ILoggingService loggingService,
            IFileSystem fileSystem)
        {
            _loggingService = loggingService;
            _fileSystem = fileSystem;
        }

        public ZippingResult ZipDirectory(string directoryPath)
        {
            var parentDir = _fileSystem.DirectoryInfo.FromDirectoryName(directoryPath).Parent.FullName;
            var zipFileName = $"Repo_{DateTime.UtcNow.ToString("yyyyMMdd")}.zip";
            var zipFilePath = _fileSystem.Path.Combine(parentDir, zipFileName);

            if (_fileSystem.File.Exists(zipFilePath))
            {
                _fileSystem.File.Delete(zipFilePath);
            }

            _loggingService.LogInformation($"Starting to zip Repo to {zipFilePath}..");
            ZipFile.CreateFromDirectory(directoryPath, zipFilePath, CompressionLevel.Optimal, false);
            _loggingService.LogInformation($"Repo zipped..");

            return new ZippingResult(zipFilePath);
        }
    }
}