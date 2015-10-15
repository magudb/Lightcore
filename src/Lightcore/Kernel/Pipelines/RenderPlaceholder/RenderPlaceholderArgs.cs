using System.IO;
using Lightcore.Kernel.Data;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;

namespace Lightcore.Kernel.Pipelines.RenderPlaceholder
{
    public class RenderPlaceholderArgs : PipelineArgs
    {
        public RenderPlaceholderArgs(HttpContext httpContext, RouteData routeData, Item item, string name, TextWriter writer)
        {
            HttpContext = httpContext;
            RouteData = routeData;
            Item = item;
            Name = name;
            Writer = writer;
        }

        public HttpContext HttpContext { get; }
        public RouteData RouteData { get; }
        public Item Item { get; }
        public string Name { get; }
        public TextWriter Writer { get; }
    }
}