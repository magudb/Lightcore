namespace Lightcore.Kernel.Pipeline.Request.Processor
{
    public class FilterRequestProcessor : Kernel.Pipeline.Processor
    {
        public override void Process(PipelineArgs args)
        {
            // TODO: Look into why we get requests to forexample "favicon.ico" while static files is not enabled?
            var ignoreRequest = args.Context.Request.Path.Value.Contains(".");

            if (ignoreRequest)
            {
                args.Abort();
            }
        }
    }
}