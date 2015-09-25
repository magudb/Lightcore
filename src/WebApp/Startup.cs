using System;
using System.Linq;
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
            var languages = new[] { "en-US", "da-DK" };

            // Create context
            app.Use(async (ctx, next) =>
            {
                ctx.Items["LCC"] = new LightcoreContext();

                await next();
            });

            // Resolve language
            app.Use(async (ctx, next) =>
            {
                var light = ctx.LightcoreContext();
                var languageSegment = ctx.Request.Path.Value.ToLowerInvariant().Split('/').Skip(1).FirstOrDefault();

                // Get current language from path
                if (!string.IsNullOrWhiteSpace(languageSegment) && languages.Contains(languageSegment, StringComparer.OrdinalIgnoreCase))
                {
                    light.Language = new Language(languageSegment);

                    ctx.Request.Path = new PathString(ctx.Request.Path.Value.Replace("/" + languageSegment, ""));
                }
                else
                {
                    // Or use the default language
                    light.Language = Language.Default;
                }

                await next();
            });

            // Set some headers
            app.Use(async (ctx, next) =>
            {
                var light = ctx.LightcoreContext();

                ctx.Response.Headers.Append("X-Language", light.Language.Name);

                await next();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{*path}", new
                {
                    controller = "Home",
                    action = "Index"
                });
            });
        }
    }
}