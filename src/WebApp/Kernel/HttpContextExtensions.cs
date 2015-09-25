using Microsoft.AspNet.Http;

namespace WebApp.Kernel
{
    public static class HttpContextExtensions
    {
        public static Context LightcoreContext(this HttpContext ctx)
        {
            return (Context)ctx.Items["LCC"];
        }
    }
}