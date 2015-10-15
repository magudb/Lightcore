using System;
using System.Threading.Tasks;
using Lightcore.Kernel.Data;

namespace Lightcore.Kernel.Pipelines.RenderField.Processors
{
    public class RenderLinkFieldProcessor : Processor<RenderFieldArgs>
    {
        public override async Task ProcessAsync(RenderFieldArgs args)
        {
            var field = args.Field;
            var item = args.Item;

            if (!field.Type.Equals("general link"))
            {
                return;
            }

            var link = (LinkField)field;
            var url = string.Empty;

            if (link.TargetId != Guid.Empty)
            {
                var targetItem = await args.ItemProvider.GetItemAsync(link.TargetId.ToString(), item.Language);

                if (targetItem != null)
                {
                    url = args.ItemUrlService.GetUrl(targetItem);
                }
            }
            else
            {
                url = link.TargetUrl;
            }

            args.Writer.Write("<a href=\"{0}\">", url);
            args.Writer.Write(!string.IsNullOrEmpty(link.Description) ? link.Description : url);
            args.Writer.Write("</a>");
            args.AbortPipeline();
        }
    }
}