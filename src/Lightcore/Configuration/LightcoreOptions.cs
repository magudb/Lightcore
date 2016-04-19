using Microsoft.Extensions.OptionsModel;

namespace Lightcore.Configuration
{
    public class LightcoreOptions : IOptions<LightcoreOptions>
    {
        public SitecoreOptions Sitecore { get; set; }

        public static LightcoreOptions Default => new LightcoreOptions
        {
            Sitecore = new SitecoreOptions
            {
                StartItem = "/sitecore/content/Home"
            }
        };

        public LightcoreOptions Value => this;

        public void Verify()
        {
            if (Sitecore == null)
            {
                throw new InvalidConfigurationException($"{nameof(Sitecore)} was null");
            }

            Sitecore.Verify();
        }
    }
}