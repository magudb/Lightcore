using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lightcore.Kernel.Pipelines.RenderField;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.DependencyInjection;

namespace Lightcore.Kernel.Mvc
{
    public class LightcoreHtmlHelper
    {
        private readonly IHtmlHelper _htmlHelper;
        private readonly RenderFieldPipeline _renderFieldPipeline;

        public LightcoreHtmlHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
            _renderFieldPipeline = _htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<RenderFieldPipeline>();
        }

        public HtmlString FieldValue(string name)
        {
            var context = _htmlHelper.LightcoreContext();
            var args = _renderFieldPipeline.GetArgs(context.Item, context.Item.Fields[name]);

            _renderFieldPipeline.Run(args);

            return new HtmlString(args.Raw);
        }

        public HtmlString Field(string name)
        {
            var context = _htmlHelper.LightcoreContext();
            var args = _renderFieldPipeline.GetArgs(context.Item, context.Item.Fields[name]);

            _renderFieldPipeline.Run(args);

            return args.Results;
        }

        public async Task<HtmlString> PlaceholderAsync(string name)
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