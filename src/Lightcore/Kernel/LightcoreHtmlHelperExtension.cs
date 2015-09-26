using Microsoft.AspNet.Mvc.Rendering;

namespace Lightcore.Kernel
{
    public static class LightcoreHtmlHelperExtension
    {
        public static LightcoreHtmlHelper Lightcore(this IHtmlHelper helper)
        {
            return new LightcoreHtmlHelper(helper);
        }
    }
}