using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Rendering;

namespace Lightcore.Kernel.MVC
{
    public class LightcoreHtmlHelper
    {
        private readonly IHtmlHelper _htmlHelper;

        public LightcoreHtmlHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public HtmlString Field(string name)
        {
            // TODO: Make some kind of field render thingy instead of checking each field type?

            var context = _htmlHelper.LightcoreContext();
            var item = context.Item;
            var builder = new StringBuilder();
            var field = item.Fields[name];

            if (field != null)
            {
                if (field.Type.Equals("image"))
                {
                    builder.Append("<img ");
                    builder.AppendFormat("src=\"{0}\" ", field.Value);
                    builder.Append("/>");
                }
                else
                {
                    builder.Append(field.Value);
                }
            }

            return new HtmlString(builder.ToString());
        }

        public async Task<HtmlString> Placeholder(string name)
        {
            // TODO: Make real render pipeline...

            var builder = new StringBuilder();
            var context = _htmlHelper.LightcoreContext();

            foreach (var rendering in context.Item.Visualization.Renderings.Where(r => r.Placeholder.Equals(name, StringComparison.OrdinalIgnoreCase))
                )
            {
                var runner = new ControllerRunner(rendering.Controller, rendering.Action, _htmlHelper.ViewContext.HttpContext,
                    _htmlHelper.ViewContext.RouteData);
                var output = await runner.Execute();

                builder.Append(output);
            }

            return new HtmlString(builder.ToString());
        }
    }
}