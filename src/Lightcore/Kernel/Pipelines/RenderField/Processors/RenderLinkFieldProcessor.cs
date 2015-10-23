using System;
using System.Threading.Tasks;
using Lightcore.Kernel.Data.Fields;
using Lightcore.Kernel.Url;

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

            args.Output.Write("<a");
            args.Attributes.Add("href", url);

            foreach (var option in args.Attributes)
            {
                args.Output.Write(" {0}=\"{1}\" ", option.Key, option.Value);
            }

            args.Output.Write(">");
            args.Output.Write(!string.IsNullOrEmpty(link.Description) ? link.Description : url);
            args.Output.Write("</a>");
            args.AbortPipeline();
        }
    }
}