using System;
using System.Text;
using Lightcore.Kernel.Data;
using Microsoft.AspNet.Mvc.Rendering;

namespace Lightcore.Kernel.Pipelines.RenderField.Processors
{
    public class RenderLinkFieldProcessor : Processor
    {
        public override void Process(PipelineArgs args)
        {
            var renderFieldArgs = (RenderFieldArgs)args;
            var field = renderFieldArgs.Field;
            var item = renderFieldArgs.Item;

            if (field.Type.Equals("general link"))
            {
                var link = (LinkField)field;
                var url = string.Empty;
                var builder = new StringBuilder();
                if (link.TargetId != Guid.Empty)
                {
                    var targetItem = renderFieldArgs.ItemProvider.GetItemAsync(link.TargetId.ToString(), item.Language).Result;

                    if (targetItem != null)
                    {
                        url = renderFieldArgs.ItemUrlService.GetUrl(targetItem);
                    }
                }
                else
                {
                    url = link.TargetUrl;
                }

                builder.AppendFormat("<a href=\"{0}\">", url);
                builder.Append(!string.IsNullOrEmpty(link.Description) ? link.Description : url);
                builder.Append("</a>");

                renderFieldArgs.Results = new HtmlString(builder.ToString());
                renderFieldArgs.Raw = url;
                renderFieldArgs.AbortPipeline();
            }
        }
    }
}