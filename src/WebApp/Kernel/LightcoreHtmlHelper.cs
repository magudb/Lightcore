using System.Text;
using System.Threading.Tasks;
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

        public async Task<HtmlString> Placeholder(string name)
        {
            // TODO: Make real render pipeline...

            var builder = new StringBuilder();
            var context = _htmlHelper.LightcoreContext();

            builder.Append("<div style=\"border: 1px solid red; padding:5px; margin:5px;\">");
            builder.Append($"<p>PLACEHOLDER:{name} ({context.Item.Key}, {context.Item.Id})</p>");

            foreach (var key in context.Item.Renderings.Keys)
            {
                if (!key.EndsWith(name))
                {
                    continue;
                }

                // TODO: Item.Rendering and Item.Layout should be types
                var controller = context.Item.Renderings[key];
                var runner = new ControllerRunner(controller, "Index", _htmlHelper.ViewContext.HttpContext, _htmlHelper.ViewContext.RouteData);
                var output = await runner.Execute();

                builder.Append(output);
            }

            builder.Append("</div>");

            return new HtmlString(builder.ToString());
        }
    }
}