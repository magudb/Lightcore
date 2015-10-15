using Lightcore.Kernel.Data;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;

namespace Lightcore.Kernel.Pipelines.RenderRendering
{
    public class RenderRenderingArgs : PipelineArgs
    {
        public RenderRenderingArgs(HttpContext httpContext, RouteData routeData, Rendering rendering)
        {
            HttpContext = httpContext;
            RouteData = routeData;
            Rendering = rendering;
            Results = string.Empty;
        }

        public HttpContext HttpContext { get; }
        public RouteData RouteData { get; }
        public Rendering Rendering { get; }
        public string Results { get; set; }
    }
}