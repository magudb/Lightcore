using Lightcore.Kernel.Configuration;
using Lightcore.Kernel.Data;
using Microsoft.AspNet.Http;

namespace Lightcore.Kernel.Pipelines.Startup
{
    public class StartupArgs : PipelineArgs
    {
        public StartupArgs(HttpContext httpContext, IItemProvider itemProvider, LightcoreConfig config)
        {
            HttpContext = httpContext;
            Config = config;
            ItemProvider = itemProvider;
        }

        public HttpContext HttpContext { get; }

        public LightcoreConfig Config { get; }

        public IItemProvider ItemProvider { get; }
    }
}