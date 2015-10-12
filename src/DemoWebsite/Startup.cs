using Lightcore.Hosting;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace WebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            // Build Lightcore configuration
            Configuration = appEnv.BuildLightcoreConfiguration(env);
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add Lightcore services
            services.AddLightcore(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationEnvironment appEnv, ILoggerFactory loggerFactory)
        {
            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Enable Lightcore
            app.UseLightcore();
        }
    }
}