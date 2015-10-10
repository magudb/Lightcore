using System.Threading.Tasks;
using Lightcore.Kernel.Configuration;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Pipeline.Request;
using Lightcore.Kernel.Pipeline.Startup;
using Lightcore.Server;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;

namespace Lightcore.Hosting
{
    public static class StartupExtensions
    {
        public static IConfiguration BuildLightcoreConfiguration(this IApplicationEnvironment appEnv, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder(appEnv.ApplicationBasePath);

            // Add main config file
            builder.AddJsonFile("lightcore.json");

            // Add optional environment specific config file, will be merged and duplicates takes precedence
            builder.AddJsonFile($"lightcore.{env.EnvironmentName}.json", true);

            // Add environment variables, will be merged and duplicates takes precedence - Set LightcoreConfig:ServerUrl in Azure for example...
            builder.AddEnvironmentVariables();

            return builder.Build();
        }

        public static void AddLightcore(this IServiceCollection services, IConfiguration config)
        {
            // Add MVC services
            services.AddMvc();

            // Add Lightcore configuration so it can be used as a dependency IOptions<LightcoreConfig> config
            services.Configure<LightcoreConfig>(config.GetSection("LightcoreConfig"));

            // Add Lightcore services
            services.AddSingleton<IItemProvider, LightcoreApiItemProvider>();
            services.AddSingleton<StartupPipeline>();
            services.AddSingleton<RequestPipeline>();
        }

        public static IApplicationBuilder UseLightcore(this IApplicationBuilder app)
        {
            var startupPipeline = app.ApplicationServices.GetRequiredService<StartupPipeline>();
            
            // Add startup pipeline
            app.Use(async (httpContext, next) =>
            {
                // TODO: Handle pipeline exceptions
                startupPipeline.Run(startupPipeline.GetArgs(httpContext));

                if (!startupPipeline.IsAborted)
                {
                    await next();
                }
            });

            var requestPipeline = app.ApplicationServices.GetRequiredService<RequestPipeline>();

            // Add request pipeline
            app.Use(async (httpContext, next) =>
            {
                // TODO: Handle pipeline exceptions
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