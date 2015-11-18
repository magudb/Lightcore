using System.Text;
using Lightcore.Configuration;
using Lightcore.Kernel.Data.Globalization;
using Microsoft.Extensions.OptionsModel;

namespace Lightcore.Kernel.Url
{
    public class ItemUrlService : IItemUrlService
    {
        private readonly LightcoreOptions _options;

        public ItemUrlService(IOptions<LightcoreOptions> options)
        {
            _options = options.Value;
        }

        public string GetUrl(Language language, string path)
        {
            Requires.IsNotNull(language, nameof(language));
            Requires.IsNotNullOrEmpty(path, nameof(path));

            var builder = new StringBuilder();

            builder.Append("/");
            builder.Append(language.Name);
            builder.Append(path.ToLowerInvariant().Replace(_options.Sitecore.StartItem.ToLowerInvariant(), ""));

            return builder.ToString();
        }
    }
}