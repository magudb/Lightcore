using Lightcore.Http.Configuration;
using Lightcore.Kernel.Data.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lightcore.Http
{
    public static class StartupExtensions
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration config)
        {
           services.Configure<HttpItemProviderOptions>(config.GetSection("HttpItemProvider"));
           services.AddSingleton<IItemProvider, HttpItemProvider>();
        }
    }
}