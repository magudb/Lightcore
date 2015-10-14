using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Routing;

namespace Lightcore.Kernel.Mvc
{
    public class ControllerRunner
    {
        private readonly string _action;
        private readonly string _controller;
        private readonly HttpContext _httpContext;
        private readonly RouteData _routeData;

        public ControllerRunner(string controller, string action, HttpContext httpContext, RouteData routeData)
        {
            _controller = controller;
            _action = action;
            _httpContext = httpContext;
            _routeData = routeData;
        }

        public async Task<string> Execute()
        {
            string output;

            var routeContext = new RouteContext(_httpContext)
            {
                RouteData = new RouteData(_routeData)
            };

            // Save context
            var currentOutputStream = _httpContext.Response.Body;
            var currentRouteData = routeContext.RouteData;

            using (var outputStream = new MemoryStream())
            {
                // Setup context
                _httpContext.Response.Body = outputStream;

                routeContext.RouteData.Values["controller"] = _controller;
                routeContext.RouteData.Values["action"] = _action;

                var handler = new MvcRouteHandler();

                try
                {
                    // Invoke controller
                    await handler.RouteAsync(routeContext);

                    outputStream.Position = 0;

                    using (var reader = new StreamReader(outputStream))
                    {
                        output = reader.ReadToEnd();
                    }
                }
                finally
                {
                    // Restore context
                    routeContext.RouteData = currentRouteData;

                    _httpContext.Response.Body = currentOutputStream;
                }
            }

            return output;
        }
    }
}