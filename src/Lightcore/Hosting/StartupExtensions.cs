using System;
using System.Net.Http;
using Lightcore.Kernel.Configuration;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Pipeline.Request;
using Lightcore.Server;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
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

            var client = new HttpClient();
           
            // Map Lightcore media handler
            app.Map("/-/media", builder =>
            {
                builder.Run(async httpContext =>
                {
                    // TODO: The only case I can see where I want to do this is to keep the Sitecore server internal ... else use media directly...

                    // TODO: Should this be a dedicted media request pipeline? 

                    // TODO: Very stange, whenever some cs file is saved, stream does not work anymore...

                    // TODO: GetMediaStreamAsync should return Media object with steam and mimetype

                    var pathWithoutExtension = httpContext.Request.Path.ToString().Replace(".jpg", "");
                    var url = "http://lightcore-testserver.ad.codehouse.com/~/media" + pathWithoutExtension + "?sc_database=web";
                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        using (var media = await response.Content.ReadAsStreamAsync())
                        {
                            media.Position = 0;

                            httpContext.Response.ContentLength = media.Length;
                            httpContext.Response.ContentType = response.Content.Headers.ContentType.MediaType;

                            // TODO: Set caching headers

                            await media.CopyToAsync(httpContext.Response.Body);
                        }
                    }
                });
            });

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