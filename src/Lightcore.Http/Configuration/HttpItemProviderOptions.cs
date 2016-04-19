using Microsoft.Extensions.OptionsModel;

namespace Lightcore.Http.Configuration
{
    public class HttpItemProviderOptions : IOptions<HttpItemProviderOptions>
    {
        public string ServerUrl { get; set; }

        public SitecoreOptions Sitecore { get; set; }

        public static HttpItemProviderOptions Default => new HttpItemProviderOptions
        {
            Sitecore = new SitecoreOptions
            {
                Database = "web",
                Device = "default",
                Cdn = null
            }
        };

        public HttpItemProviderOptions Value => this;
    }
}