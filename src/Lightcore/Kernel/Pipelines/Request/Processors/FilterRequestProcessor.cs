namespace Lightcore.Kernel.Pipelines.Request.Processors
{
    public class FilterRequestProcessor : Processor<RequestArgs>
    {
        public override void Process(RequestArgs args)
        {
            var ignoreRequest = args.HttpContext.Request.Path.Value.Contains(".");

            if (ignoreRequest)
            {
                args.AbortPipeline();
            }
        }
    }
}