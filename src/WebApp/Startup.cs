using System;
using System.Collections.Generic;
using System.Linq;
using Lightcore.Kernel;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;

namespace WebApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            var languages = new[] {"en-US", "da-DK"};

            // Create context
            app.Use(async (httpContext, next) =>
            {
                httpContext.Items["LCC"] = new Context();

                await next();
            });

            // Resolve language
            app.Use(async (httpContext, next) =>
            {
                var context = httpContext.LightcoreContext();
                var languageSegment = httpContext.Request.Path.Value.ToLowerInvariant().Split('/').Skip(1).FirstOrDefault();

                // Get current language from path
                if (!string.IsNullOrWhiteSpace(languageSegment) && languages.Contains(languageSegment, StringComparer.OrdinalIgnoreCase))
                {
                    context.Language = new Language(languageSegment);

                    httpContext.Request.Path = new PathString(httpContext.Request.Path.Value.Replace("/" + languageSegment, ""));
                }
                else
                {
                    // Or use the default language
                    context.Language = Language.Default;
                }

                await next();
            });

            // Resolve item
            app.Use(async (httpContext, next) =>
            {
                var context = httpContext.LightcoreContext();

                context.Item = new Item
                {
                    Id = Guid.NewGuid(),
                    Key = "Dell",
                    Path = httpContext.Request.Path.Value.ToLowerInvariant(),
                    Language = context.Language,
                    Layout = "/Views/Layout.cshtml",
                    Renderings = new Dictionary<string, string>
                    {
                        ["content"] = "Article",
                        ["footer"] = "Footer"
                    }
                };

                await next();
            });

            // Add some headers, just for fun...
            app.Use(async (httpContext, next) =>
            {
                var context = httpContext.LightcoreContext();

                httpContext.Response.Headers.Append("X-Language", context.Language.Name);

                await next();
            });

            // Enabled MVC
            app.UseMvc(routes =>
            {
                // Route to handle controller that invokes item layout
                routes.MapRoute("default", "{*path}", new
                {
                    controller = "Lightcore",
                    action = "Render"
                });
            });
        }
    }
}