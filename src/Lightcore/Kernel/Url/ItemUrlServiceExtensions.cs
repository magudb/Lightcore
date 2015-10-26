using Lightcore.Kernel.Data;

namespace Lightcore.Kernel.Url
{
    public static class UrlExtensions
    {
        public static string GetUrl(this IItemUrlService service, IItemDefinition item)
        {
            Requires.IsNotNull(item, nameof(item));

            return service.GetUrl(item.Language, item.Path);
        }
    }
}