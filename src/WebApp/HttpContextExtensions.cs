using Microsoft.AspNet.Http;

namespace WebApp
{
    public static class HttpContextExtensions
    {
        public static LightcoreContext LightcoreContext(this HttpContext ctx)
        {
            return (LightcoreContext)ctx.Items["LCC"];
        }
    }
}