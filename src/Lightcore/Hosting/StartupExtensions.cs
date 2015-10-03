using Lightcore.Kernel.Configuration;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Pipeline;
using Lightcore.Kernel.Pipeline.Request;
using Lightcore.Server;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;

namespace Lightcore.Hosting
{
    public static class StartupExtensions
    {
        public static IConfigurationBuilder ConfigureLightcore(this IConfigurationBuilder builder, IHostingEnvironment env)
        {
            // Add main config file
            builder.AddJsonFile("lightcore.json");

            // Add optional environment specific config file, will be merged and duplicates takes precedence
            builder.AddJsonFile($"lightcore.{env.EnvironmentName}.json", true);

            // Add environment variables, will be merged and duplicates takes precedence - Set LightcoreConfig:ServerUrl in Azure for example...
            builder.AddEnvironmentVariables();

            return builder;
        }

        public static void AddLightcore(this IServiceCollection services, IConfiguration config)
        {
            services.AddCaching();
            services.Configure<LightcoreConfig>(config.GetSection("LightcoreConfig"));
            services.AddSingleton<IItemProvider, LightcoreApiItemProvider>();
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