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

            args.Output.Write("<img");
            args.Attributes.Add("src", image.Url);

            if (!string.IsNullOrEmpty(image.Alt))
            {
                args.Attributes.Add("alt", image.Alt);
            }

            foreach (var option in args.Attributes)
            {
                args.Output.Write(" {0}=\"{1}\"", option.Key, option.Value);
            }

            args.Output.Write("/>");
            args.AbortPipeline();
        }
    }
}