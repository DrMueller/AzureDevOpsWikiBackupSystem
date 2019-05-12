using Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.Zipping.Models;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.Zipping.Services
{
    public interface IZippingService
    {
        ZippingResult ZipDirectory(string directoryPath);
    }
}