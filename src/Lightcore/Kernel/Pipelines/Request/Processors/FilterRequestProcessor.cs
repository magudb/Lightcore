namespace Lightcore.Kernel.Pipelines.Request.Processors
{
    public class FilterRequestProcessor : Processor
    {
        public override void Process(PipelineArgs args)
        {
            var requestArgs = (RequestArgs)args;

            // TODO: Look into why we get requests to forexample "favicon.ico" while static files is not enabled?
            var ignoreRequest = requestArgs.HttpContext.Request.Path.Value.Contains(".");

            if (ignoreRequest)
            {
                args.Abort();
            }
        }
    }
}