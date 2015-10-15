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

            args.Output.Write("<img src=\"{0}\"", image.Url);

            if (!string.IsNullOrEmpty(image.Alt))
            {
                args.Output.Write(" alt=\"{0}\"", image.Alt);
            }

            args.Output.Write("/>");
            args.AbortPipeline();
        }
    }
}