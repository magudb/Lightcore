using Lightcore.Kernel.Data;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Routing;

namespace Lightcore.Kernel.Pipelines.RenderPlaceholder
{
    public class RenderPlaceholderArgs : PipelineArgs
    {
        public RenderPlaceholderArgs(HttpContext httpContext, RouteData routeData, Item item, string name)
        {
            HttpContext = httpContext;
            RouteData = routeData;
            Item = item;
            Name = name;
        }

        public HttpContext HttpContext { get; }
        public RouteData RouteData { get; }
        public Item Item { get; }
        public string Name { get; }
        public HtmlString Results { get; set; }
    }
}