using Mmu.Mlh.LanguageExtensions.Areas.Invariance;

namespace Mmu.AzureDevOpsWikiBackupSystem.Areas.SubAreas.Zipping.Models
{
    public class ZippingResult
    {
        public string ZipFilePath { get; }

        public ZippingResult(string zipFilePath)
        {
            Guard.StringNotNullOrEmpty(() => zipFilePath);
            ZipFilePath = zipFilePath;
        }
    }
}