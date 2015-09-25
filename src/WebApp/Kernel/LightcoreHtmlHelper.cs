using Microsoft.AspNet.Mvc.Rendering;

namespace WebApp.Kernel
{
    public class LightcoreHtmlHelper
    {
        private readonly IHtmlHelper _htmlHelper;

        public LightcoreHtmlHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public HtmlString Placeholder(string name)
        {
            var context = _htmlHelper.LightcoreContext();

            return new HtmlString($"<p>PLACEHOLDER:{name} ({context.Item.Key}, {context.Item.Id})</p>");
        }
    }
}