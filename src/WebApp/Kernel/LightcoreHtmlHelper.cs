using Microsoft.AspNet.Mvc.Rendering;

namespace WebApp.Kernel
{
    public class LightcoreHtmlHelper
    {
        public HtmlString Placeholder(string name)
        {
            return new HtmlString($"<p>PLACEHOLDER:{name}</p>");
        }
    }
}