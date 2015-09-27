namespace Lightcore.Kernel.Pipeline.Request.Processor
{
    public class CreateContextProcessor : Kernel.Pipeline.Processor
    {
        public override void Process(PipelineArgs args)
        {
            args.Context.Items["LCC"] = new Context();
        }
    }
}