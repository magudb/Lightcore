using Microsoft.AspNet.Http;

namespace Lightcore.Kernel
{
    public static class HttpContextExtensions
    {
        public static Context LightcoreContext(this HttpContext ctx)
        {
            return (Context)ctx.Items["LCC"];
        }
    }
}