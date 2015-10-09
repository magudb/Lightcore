using Lightcore.Kernel.Configuration;
using Lightcore.Kernel.Data;
using Microsoft.AspNet.Http;

namespace Lightcore.Kernel.Pipeline.Startup
{
    public class StartupArgs : PipelineArgs
    {
        public StartupArgs(HttpContext httpContext, IItemProvider itemProvider, LightcoreConfig config) : base(httpContext)
        {
            Config = config;
            ItemProvider = itemProvider;
        }

        public LightcoreConfig Config { get; }

        public IItemProvider ItemProvider { get; }
    }
}