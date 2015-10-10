using Lightcore.Kernel.Configuration;
using Lightcore.Kernel.Data;
using Microsoft.AspNet.Http;

namespace Lightcore.Kernel.Pipeline.Request
{
    public class RequestArgs : PipelineArgs
    {
        public RequestArgs(HttpContext httpContext, IItemProvider itemProvider, LightcoreConfig config)
        {
            HttpContext = httpContext;
            Config = config;
            ItemProvider = itemProvider;
        }

        public HttpContext HttpContext { get; set; }

        public LightcoreConfig Config { get; }

        public IItemProvider ItemProvider { get; }
    }
}