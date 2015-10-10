using System.Text;
using Lightcore.Kernel.Configuration;
using Lightcore.Kernel.Data;
using Microsoft.Framework.OptionsModel;

namespace Lightcore.Kernel.Urls
{
    public class ItemUrlService : IItemUrlService
    {
        private readonly LightcoreOptions _options;

        public ItemUrlService(IOptions<LightcoreOptions> options)
        {
            _options = options.Options;
        }

        public string GetUrl(Item item)
        {
            var builder = new StringBuilder();

            builder.Append("/");
            builder.Append(item.Language.Name);
            builder.Append(item.Path.ToLowerInvariant().Replace(_options.Sitecore.StartItem.ToLowerInvariant(), ""));

            return builder.ToString();
        }
    }
}