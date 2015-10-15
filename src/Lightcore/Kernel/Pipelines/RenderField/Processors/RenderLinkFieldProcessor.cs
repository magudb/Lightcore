using System;
using System.Text;
using Lightcore.Kernel.Data;

namespace Lightcore.Kernel.Pipelines.RenderField.Processors
{
    public class RenderLinkFieldProcessor : Processor<RenderFieldArgs>
    {
        public override void Process(RenderFieldArgs args)
        {
            var field = args.Field;
            var item = args.Item;

            if (!field.Type.Equals("general link"))
            {
                return;
            }

            var link = (LinkField)field;
            var url = string.Empty;
            var builder = new StringBuilder();
            if (link.TargetId != Guid.Empty)
            {
                var targetItem = args.ItemProvider.GetItemAsync(link.TargetId.ToString(), item.Language).Result;

                if (targetItem != null)
                {
                    url = args.ItemUrlService.GetUrl(targetItem);
                }
            }
            else
            {
                url = link.TargetUrl;
            }

            builder.AppendFormat("<a href=\"{0}\">", url);
            builder.Append(!string.IsNullOrEmpty(link.Description) ? link.Description : url);
            builder.Append("</a>");

            args.Results = builder.ToString();
            args.AbortPipeline();
        }
    }
}