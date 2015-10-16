using Microsoft.Framework.OptionsModel;

namespace Lightcore.Kernel.Configuration
{
    public class LightcoreOptions : IOptions<LightcoreOptions>
    {
        public string ServerUrl { get; set; }

        public SitecoreOptions Sitecore { get; set; }

        public static LightcoreOptions Default => new LightcoreOptions
        {
            Sitecore = new SitecoreOptions
            {
                StartItem = "/sitecore/content/Home",
                Database = "web",
                Device = "default"
            }
        };

        public LightcoreOptions Value => this;

        public void Verify()
        {
            if (Sitecore == null)
            {
                throw new InvalidConfigurationException($"{nameof(Sitecore)} was null");
            }

            if (string.IsNullOrEmpty(ServerUrl))
            {
                throw new InvalidConfigurationException($"{nameof(ServerUrl)} was empty");
            }
        }
    }
}