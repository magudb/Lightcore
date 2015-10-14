using System.Threading.Tasks;
using Lightcore.Kernel.Pipelines.RenderField;
using Lightcore.Kernel.Pipelines.RenderPlaceholder;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.DependencyInjection;

namespace Lightcore.Kernel.Mvc
{
    public class LightcoreHtmlHelper
    {
        private readonly IHtmlHelper _htmlHelper;
        private readonly RenderFieldPipeline _renderFieldPipeline;
        private readonly RenderPlaceholderPipeline _renderPlaceholderPipeline;

        public LightcoreHtmlHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
            _renderFieldPipeline = _htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<RenderFieldPipeline>();
            _renderPlaceholderPipeline = _htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<RenderPlaceholderPipeline>();
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
            var context = _htmlHelper.LightcoreContext();
            var args = _renderPlaceholderPipeline.GetArgs(_htmlHelper.ViewContext.HttpContext, _htmlHelper.ViewContext.RouteData, context.Item, name);

            await _renderPlaceholderPipeline.RunAsync(args);

            return args.Results;
        }
    }
}