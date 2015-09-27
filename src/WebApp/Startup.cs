using Lightcore.Hosting;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;

namespace WebApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Lightcore services
            services.AddLightcore();
            // TODO: Implement .AddSitecoreConnection("http://sc72-141226.ad.codehouse.com"); or something for config

            // Add MVC services
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            // Enable Lightcore
            app.UseLightcore();

            // Enabled MVC
            app.UseMvc(routes =>
            {
                // Enable Lightcore, replace default route
                routes.MapRoute("default", "{*path}", new
                {
                    controller = "Lightcore",
                    action = "Render"
                });
            });
        }
    }
}