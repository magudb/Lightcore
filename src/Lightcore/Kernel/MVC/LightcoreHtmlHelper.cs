using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Lightcore.Kernel.Pipelines.RenderField;
using Lightcore.Kernel.Pipelines.RenderPlaceholder;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Routing;
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
            _renderFieldPipeline = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<RenderFieldPipeline>();
            _renderPlaceholderPipeline = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<RenderPlaceholderPipeline>();
        }

        public HtmlString Field(string name)
        {
            var context = _htmlHelper.LightcoreContext();

            return new HtmlString(context.Item[name]);
        }

        public HtmlString RenderField(string name, dynamic attributes = null)
        {
            using (var writer = new StringWriter())
            {
                var context = _htmlHelper.LightcoreContext();
                var args = _renderFieldPipeline.GetArgs(context.Item, context.Item.Fields[name], writer, ToDictionary(attributes));

                _renderFieldPipeline.RunAsync(args).Wait();

                return new HtmlString(args.Output.ToString());
            }
        }

        public async Task<HtmlString> PlaceholderAsync(string name)
        {
            using (var writer = new StringWriter())
            {
                var context = _htmlHelper.LightcoreContext();
                var args = _renderPlaceholderPipeline.GetArgs(_htmlHelper.ViewContext.HttpContext,
                    _htmlHelper.ViewContext.RouteData,
                    context.Item,
                    name,
                    writer);

                await _renderPlaceholderPipeline.RunAsync(args);

                return new HtmlString(args.Output.ToString());
            }
        }

        private Dictionary<string, string> ToDictionary(dynamic @object)
        {
            var dictionary = new Dictionary<string, string>();

            if (@object == null)
            {
                return dictionary;
            }

            var converter = new RouteValueDictionary(@object);

            foreach (var key in converter.Keys)
            {
                dictionary.Add(key, converter[key].ToString());
            }

            return dictionary;
        }
    }
}