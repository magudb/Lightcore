using System.Text;
using Lightcore.Kernel.Data;
using Microsoft.AspNet.Mvc.Rendering;

namespace Lightcore.Kernel.Pipelines.RenderField.Processors
{
    public class RenderMediaFieldProcessor : Processor<RenderFieldArgs>
    {
        public override void Process(RenderFieldArgs args)
        {
            var field = args.Field;

            if (!field.Type.Equals("image"))
            {
                return;
            }

            var builder = new StringBuilder();
            var image = (ImageField)field;

            builder.AppendFormat("<img src=\"{0}\"", image.Url);

            if (!string.IsNullOrEmpty(image.Alt))
            {
                builder.AppendFormat(" alt=\"{0}\"", image.Alt);
            }

            builder.Append("/>");

            args.Results = new HtmlString(builder.ToString());
            args.AbortPipeline();
        }
    }
}