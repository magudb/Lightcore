namespace Lightcore.Kernel.Pipeline.Request.Processor
{
    public class FilterRequestProcessor : Kernel.Pipeline.Processor
    {
        public override void Process(PipelineArgs args)
        {
            var ignoreRequest = args.Context.Request.Path.Value.Contains(".");

            if (ignoreRequest)
            {
                args.Abort();
            }
        }
    }
}