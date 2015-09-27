using Lightcore.Kernel.Data;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;

namespace Lightcore.Hosting
{
    public static class StartupExtensions
    {
        public static void AddLightcore(this IServiceCollection services)
        {
            services.AddInstance<IItemProvider>(new ItemProvider());
        }

        public static IApplicationBuilder UseLightcore(this IApplicationBuilder app)
        {
            // TODO: Throw if services not added...

            return app;
        }
    }
}