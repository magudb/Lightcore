using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Routing;
using Microsoft.Net.Http.Server;

namespace WebApp.Kernel
{
    public class LightcoreHtmlHelper
    {
        private readonly IHtmlHelper _htmlHelper;

        public LightcoreHtmlHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public async Task<HtmlString> Placeholder(string name)
        {
            // TODO: Run pipeline...

            var builder = new StringBuilder();
            var context = _htmlHelper.LightcoreContext();

            builder.Append("<div style=\"border: 1px solid red; padding:5px; margin:5px;\">");
            builder.Append($"<p>PLACEHOLDER:{name} ({context.Item.Key}, {context.Item.Id})</p>");

            foreach (var key in context.Item.Renderings.Keys)
            {
                if (!key.EndsWith(name))
                {
                    continue;
                }

                var httpContext = _htmlHelper.ViewContext.HttpContext;
                var rendering = context.Item.Renderings[key];
                var renderingOutput = "";

                builder.Append($"<p>Adding rendering '{rendering}'...</p>");

                var currentOutput = httpContext.Response.Body;
                var routeContext = new RouteContext(httpContext)
                {
                    RouteData = new RouteData(_htmlHelper.ViewContext.RouteData)
                };

                var currentController = routeContext.RouteData.Values["controller"];
                var currentAction = routeContext.RouteData.Values["action"];

                using (var responseStream = new MemoryStream())
                {
                    var handler = new MvcRouteHandler();

                    httpContext.Response.Body = responseStream;
                    routeContext.RouteData.Values["controller"] = rendering;
                    routeContext.RouteData.Values["action"] = "Index";

                    try
                    {
                        await handler.RouteAsync(routeContext); // Second request throws an exception!?!?!

                        responseStream.Position = 0;

                        using (var reader = new StreamReader(responseStream))
                        {
                            renderingOutput = reader.ReadToEnd();
                        }
                    }
                    finally
                    {
                        routeContext.RouteData.Values["controller"] = currentController;
                        routeContext.RouteData.Values["action"] = currentAction;
                        httpContext.Response.Body = currentOutput;
                    }
                }

                builder.Append(renderingOutput);
            }

            builder.Append("</div>");

            return new HtmlString(builder.ToString());
        }
    }
}