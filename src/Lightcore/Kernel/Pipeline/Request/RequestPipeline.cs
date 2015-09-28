using Lightcore.Kernel.Pipeline.Request.Processor;

namespace Lightcore.Kernel.Pipeline.Request
{
    public class RequestPipeline : Pipeline
    {
        public RequestPipeline()
        {
            Add(new FilterRequestProcessor());
            Add(new CreateContextProcessor());
            Add(new ResolveLanguageProcessor());
            Add(new ResolveItemProcessor());
        }
    }
}