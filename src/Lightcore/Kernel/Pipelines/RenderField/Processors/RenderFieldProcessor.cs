namespace Lightcore.Kernel.Pipelines.RenderField.Processors
{
    public class RenderFieldProcessor : Processor<RenderFieldArgs>
    {
        public override void Process(RenderFieldArgs args)
        {
            args.Writer.Write(args.Field.Value);
            args.AbortPipeline();
        }
    }
}