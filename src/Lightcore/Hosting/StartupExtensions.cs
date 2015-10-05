using System.Net.Http;
using Lightcore.Kernel.Configuration;
using Lightcore.Kernel.Data;
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
            // Add MVC services
            services.AddMvc();

            // Add Lightcore configuration so it can be used as a dependency IOptions<LightcoreConfig> config
            services.Configure<LightcoreConfig>(config.GetSection("LightcoreConfig"));

            // Add Lightcore services
            services.AddSingleton<IItemProvider, LightcoreApiItemProvider>();
            services.AddTransient<RequestPipeline>();
        }

        public static IApplicationBuilder UseLightcore(this IApplicationBuilder app)
        {
            // TODO: Throw if services not added (See Mvc, they use a marker service as the last registered and check that)...

            var requestPipeline = app.ApplicationServices.GetService<RequestPipeline>();

            // Use Lightcore item pipelines
            app.Use(async (httpContext, next) =>
            {
                await requestPipeline.RunAsync(requestPipeline.GetArgs(httpContext));

                if (!requestPipeline.IsAborted)
                {
                    await next();
                }
            });

            // Enabled MVC
            app.UseMvc(routes =>
            {
                // Replace default route with Lightcore controller
                routes.MapRoute("default", "{*path}", new
                {
                    controller = "Lightcore",
                    action = "Render"
                });
            });

            return app;
        }
    }
}