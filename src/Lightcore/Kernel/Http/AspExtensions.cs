using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Rendering;

namespace Lightcore.Kernel.Http
{
    public static class AspExtensions
    {
        public static Context LightcoreContext(this HttpContext ctx)
        {
            return (Context)ctx.Items["LCC"];
        }

        public static Context LightcoreContext(this IHtmlHelper helper)
        {
            return helper.ViewContext.HttpContext.LightcoreContext();
        }
    }
}