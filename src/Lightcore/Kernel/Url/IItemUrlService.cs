using Lightcore.Kernel.Data.Globalization;

namespace Lightcore.Kernel.Url
{
    public interface IItemUrlService
    {
        string GetUrl(Language language, string path);
    }
}