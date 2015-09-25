using Microsoft.AspNet.Mvc.Rendering;

namespace WebApp.Kernel
{
    public static class LightcoreHtmlHelperExtension
    {
        private static LightcoreHtmlHelper _helper;

        public static LightcoreHtmlHelper Lightcore(this IHtmlHelper helper)
        {
            return _helper ?? (_helper = new LightcoreHtmlHelper(helper));
        }
    }
}