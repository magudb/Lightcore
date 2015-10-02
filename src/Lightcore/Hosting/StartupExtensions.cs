using Lightcore.Kernel.Data;
using Lightcore.Kernel.Pipeline;
using Lightcore.Kernel.Pipeline.Request;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;

namespace Lightcore.Hosting
{
    public static class StartupExtensions
    {
        public static void AddLightcore(this IServiceCollection services)
        {
            //services.AddInstance<IItemProvider>(new ItemWebApiItemProvider());
            services.AddInstance<IItemProvider>(new LightcoreApiItemProvider());
            services.AddTransient<RequestPipeline>();
        }

        public static IApplicationBuilder UseLightcore(this IApplicationBuilder app)
        {
            // TODO: Throw if services not added...

            var itemProvider = app.ApplicationServices.GetService<IItemProvider>();
            var requestPipeline = app.ApplicationServices.GetService<RequestPipeline>();

            app.Use(async (httpContext, next) =>
            {
                await requestPipeline.RunAsync(new PipelineArgs(httpContext, itemProvider));

                if (!requestPipeline.IsAborted)
                {
                    await next();
                }
            });

            return app;
        }
    }
}