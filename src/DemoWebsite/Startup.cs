﻿using Lightcore.Hosting;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace DemoWebsite
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
            // Use detailed exception page
            app.UseDeveloperExceptionPage();

            // Add the platform handler to the request pipeline.
            app.UseIISPlatformHandler();

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Example of using native MVC without running the Lightcore request pipeline
            app.Map("/api", builder => { app.UseMvc(); });

            // Enable Lightcore
            app.UseLightcore();
        }
    }
}