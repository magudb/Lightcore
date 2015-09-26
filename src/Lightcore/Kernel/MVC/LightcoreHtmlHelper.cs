using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lightcore.Kernel.Http;
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

        public async Task<HtmlString> Placeholder(string name)
        {
            // TODO: Make real render pipeline...

            var builder = new StringBuilder();
            var context = _htmlHelper.LightcoreContext();

            foreach (var rendering in context.Item.Renderings.Where(r => r.Placeholder.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                var runner = new ControllerRunner(rendering.Controller, rendering.Action, _htmlHelper.ViewContext.HttpContext, _htmlHelper.ViewContext.RouteData);
                var output = await runner.Execute();

                builder.Append(output);
            }

            return new HtmlString(builder.ToString());
        }
    }
}