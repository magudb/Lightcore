using System.IO;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Data.Presentation;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;

namespace Lightcore.Kernel.Pipelines.RenderRendering
{
    public class RenderRenderingArgs : PipelineArgs
    {
        public RenderRenderingArgs(HttpContext httpContext, RouteData routeData, Item item, Rendering rendering, TextWriter output)
        {
            HttpContext = httpContext;
            RouteData = routeData;
            Item = item;
            Rendering = rendering;
            Output = output;
        }

        public HttpContext HttpContext { get; }
        public RouteData RouteData { get; }
        public Item Item { get; set; }
        public Rendering Rendering { get; }
        public string CacheableOutput { get; set; }
        public TextWriter Output { get; }
        public string CacheKey { get; set; }
    }
}