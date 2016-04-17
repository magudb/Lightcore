using Lightcore.Kernel.Data.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lightcore.Redis
{
    public static class StartupExtensions
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<RedisOptions>(config.GetSection("RedisItemProvider"));
            services.AddSingleton<IItemProvider, RedisItemProvider>();
        }
    }
}