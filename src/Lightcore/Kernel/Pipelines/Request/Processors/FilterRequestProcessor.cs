namespace Lightcore.Kernel.Pipelines.Request.Processors
{
    public class FilterRequestProcessor : Processor
    {
        public override void Process(PipelineArgs args)
        {
            var requestArgs = (RequestArgs)args;
            var ignoreRequest = requestArgs.HttpContext.Request.Path.Value.Contains(".");

            if (ignoreRequest)
            {
                args.AbortPipeline();
            }
        }
    }
}