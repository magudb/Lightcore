using Microsoft.AspNet.Mvc.Rendering;

namespace Lightcore.Kernel.MVC
{
    public static class HtmlHelperExtension
    {
        public static LightcoreHtmlHelper Lightcore(this IHtmlHelper helper)
        {
            return new LightcoreHtmlHelper(helper);
        }

        public static Context LightcoreContext(this IHtmlHelper helper)
        {
            return helper.ViewContext.HttpContext.LightcoreContext();
        }
    }
}