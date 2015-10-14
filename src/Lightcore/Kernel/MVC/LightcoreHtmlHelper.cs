using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Urls;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.DependencyInjection;

namespace Lightcore.Kernel.Mvc
{
    public class LightcoreHtmlHelper
    {
        private readonly IHtmlHelper _htmlHelper;
        private readonly IItemProvider _itemProvider;
        private readonly IItemUrlService _itemUrlService;

        public LightcoreHtmlHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
            _itemProvider = _htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IItemProvider>();
            _itemUrlService = _htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IItemUrlService>();
        }

        public HtmlString FieldValue(string name)
        {
            // TODO: Make some kind of field render thingy instead of checking each field type?

            var context = _htmlHelper.LightcoreContext();
            var item = context.Item;

            var field = item.Fields[name];

            if (field == null)
            {
                return HtmlString.Empty;
            }

            if (field.Type.Equals("image"))
            {
                var image = (ImageField)field;

                return new HtmlString(image.Url);
            }

            if (field.Type.Equals("general link"))
            {
                var link = (LinkField)field;

                var url = string.Empty;

                if (link.TargetId != Guid.Empty)
                {
                    var targetItem = _itemProvider.GetItemAsync(link.TargetId.ToString(), item.Language).Result;

                    if (targetItem != null)
                    {
                        url = _itemUrlService.GetUrl(targetItem);
                    }
                }
                else
                {
                    url = link.TargetUrl;
                }

                return new HtmlString(url);
            }

            return new HtmlString(field.Value);
        }

        public HtmlString Field(string name)
        {
            // TODO: Make some kind of field render thingy instead of checking each field type?

            var context = _htmlHelper.LightcoreContext();
            var item = context.Item;

            var field = item.Fields[name];

            if (field == null)
            {
                return HtmlString.Empty;
            }

            if (field.Type.Equals("image"))
            {
                var builder = new StringBuilder();
                var image = (ImageField)field;

                builder.AppendFormat("<img src=\"{0}\"", image.Url);

                if (!string.IsNullOrEmpty(image.Alt))
                {
                    builder.AppendFormat(" alt=\"{0}\"", image.Alt);
                }

                builder.Append("/>");

                return new HtmlString(builder.ToString());
            }

            if (field.Type.Equals("general link"))
            {
                var builder = new StringBuilder();
                var link = (LinkField)field;

                var url = string.Empty;

                if (link.TargetId != Guid.Empty)
                {
                    var targetItem = _itemProvider.GetItemAsync(link.TargetId.ToString(), item.Language).Result;

                    if (targetItem != null)
                    {
                        url = _itemUrlService.GetUrl(targetItem);
                    }
                }
                else
                {
                    url = link.TargetUrl;
                }

                builder.AppendFormat("<a href=\"{0}\">", url);
                builder.Append(!string.IsNullOrEmpty(link.Description) ? link.Description : url);
                builder.Append("</a>");

                return new HtmlString(builder.ToString());
            }

            return new HtmlString(field.Value);
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