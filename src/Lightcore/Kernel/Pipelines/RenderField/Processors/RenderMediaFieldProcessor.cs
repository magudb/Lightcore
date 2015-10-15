using Lightcore.Kernel.Data;

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

            var image = (ImageField)field;

            args.Writer.Write("<img src=\"{0}\"", image.Url);

            if (!string.IsNullOrEmpty(image.Alt))
            {
                args.Writer.Write(" alt=\"{0}\"", image.Alt);
            }

            args.Writer.Write("/>");
            args.AbortPipeline();
        }
    }
}