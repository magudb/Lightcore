using Lightcore.Configuration;
using Lightcore.Hosting.Middleware;
using Lightcore.Kernel.Data.Providers;
using Lightcore.Kernel.Mvc;
using Lightcore.Kernel.Pipelines.RenderField;
using Lightcore.Kernel.Pipelines.RenderPlaceholder;
using Lightcore.Kernel.Pipelines.RenderRendering;
using Lightcore.Kernel.Pipelines.Request;
using Lightcore.Kernel.Pipelines.Startup;
using Lightcore.Kernel.Url;
using Lightcore.Server;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

namespace Lightcore.Hosting
{
    public static class StartupExtensions
    {
        public static IConfiguration BuildLightcoreConfiguration(this IApplicationEnvironment appEnv, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(appEnv.ApplicationBasePath);

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
            // Add standard MVC services and Lightcore ValueProvider
            services.AddMvc(options => { options.ValueProviderFactories.Add(new LightcoreValueProviderFactory()); });

            // Add standard caching
            services.AddCaching();

            // Add Lightcore configuration so it can be used as a dependency IOptions<LightcoreConfig> config
            services.Configure<LightcoreOptions>(config.GetSection("LightcoreOptions"));

            // Add Lightcore services
            services.AddSingleton<IItemProvider, LightcoreApiItemProvider>();
            services.AddSingleton<IItemUrlService, ItemUrlService>();

            // Add Lightcore pipelines
            services.AddSingleton<StartupPipeline>();
            services.AddSingleton<RequestPipeline>();
            services.AddSingleton<RenderFieldPipeline>();
            services.AddSingleton<RenderRenderingPipeline>();
            services.AddSingleton<RenderPlaceholderPipeline>();
        }

        public static IApplicationBuilder UseLightcore(this IApplicationBuilder app)
        {
            // Add pipelines that runs on each request
            app.UseMiddleware<StartupPipelineMiddleware>();
            app.UseMiddleware<OutputCacheMiddlewareX>();
            app.UseMiddleware<RequestPipelineMiddleware>();

            // Enabled standard MVC
            app.UseMvc(routes =>
            {
                routes.DefaultHandler = new LightcoreRouter(routes.DefaultHandler);

                // Configure default route to always use the Lightcore controller and call the Render action
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