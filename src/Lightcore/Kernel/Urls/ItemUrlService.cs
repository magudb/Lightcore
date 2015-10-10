using System.Text;
using Lightcore.Kernel.Configuration;
using Lightcore.Kernel.Data;
using Microsoft.Framework.OptionsModel;

namespace Lightcore.Kernel.Urls
{
    public class ItemUrlService : IItemUrlService
    {
        private readonly LightcoreConfig _config;

        public ItemUrlService(IOptions<LightcoreConfig> config)
        {
            _config = config.Options;
        }

        public string GetUrl(Item item)
        {
            var builder = new StringBuilder();

            builder.Append("/");
            builder.Append(item.Language.Name);
            builder.Append(item.Path.ToLowerInvariant().Replace(_config.Sitecore.StartItem.ToLowerInvariant(), ""));

            return builder.ToString();
        }
    }
}