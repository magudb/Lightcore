using System.Text;
using Lightcore.Kernel.Data;
using Microsoft.AspNet.Mvc.Rendering;

namespace Lightcore.Kernel.Pipelines.RenderField.Processors
{
    public class RenderMediaFieldProcessor : Processor
    {
        public override void Process(PipelineArgs args)
        {
            var renderFieldArgs = (RenderFieldArgs)args;
            var field = renderFieldArgs.Field;

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

                renderFieldArgs.Results = new HtmlString(builder.ToString());
                renderFieldArgs.Raw = image.Url;
                renderFieldArgs.AbortPipeline();
            }
        }
    }
}