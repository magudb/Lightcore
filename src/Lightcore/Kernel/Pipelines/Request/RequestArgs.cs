using Lightcore.Configuration;
using Lightcore.Kernel.Data;
using Microsoft.AspNet.Http;

namespace Lightcore.Kernel.Pipelines.Request
{
    public class RequestArgs : PipelineArgs
    {
        public RequestArgs(HttpContext httpContext, IItemProvider itemProvider, LightcoreOptions options)
        {
            HttpContext = httpContext;
            Options = options;
            ItemProvider = itemProvider;
        }

        public HttpContext HttpContext { get; set; }

        public LightcoreOptions Options { get; }

        public IItemProvider ItemProvider { get; }
    }
}