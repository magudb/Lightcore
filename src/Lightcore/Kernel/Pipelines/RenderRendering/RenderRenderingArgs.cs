using System.IO;
using Lightcore.Kernel.Data;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;

namespace Lightcore.Kernel.Pipelines.RenderRendering
{
    public class RenderRenderingArgs : PipelineArgs
    {
        public RenderRenderingArgs(HttpContext httpContext, RouteData routeData, Rendering rendering, TextWriter writer)
        {
            HttpContext = httpContext;
            RouteData = routeData;
            Rendering = rendering;
            Writer = writer;
        }

        public HttpContext HttpContext { get; }
        public RouteData RouteData { get; }
        public Rendering Rendering { get; }
        public TextWriter Writer { get; }
    }
}