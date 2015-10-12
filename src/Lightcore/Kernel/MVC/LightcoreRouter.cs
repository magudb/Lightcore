using System.Threading.Tasks;
using Microsoft.AspNet.Routing;

namespace Lightcore.Kernel.MVC
{
    public class LightcoreRouter : IRouter
    {
        private readonly IRouter _defaultHandler;

        public LightcoreRouter(IRouter defaultHandler)
        {
            _defaultHandler = defaultHandler;
        }

        public async Task RouteAsync(RouteContext context)
        {
            var lightcoreContext = context.HttpContext.LightcoreContext();

            if (lightcoreContext?.Item != null)
            {
                // Only try to run routes if Lightcore context is present, request pipeline could be aborted
                await _defaultHandler.RouteAsync(context);
            }
            else
            {
                // Make sure that the next middleware can run
                context.IsHandled = false;
            }
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return _defaultHandler.GetVirtualPath(context);
        }
    }
}