using Lightcore.Kernel.Data;
using Lightcore.Kernel.Pipeline.Request.Processor;
using Microsoft.AspNet.Http;

namespace Lightcore.Kernel.Pipeline.Request
{
    public class RequestPipeline : Pipeline
    {
        private readonly IItemProvider _itemProvider;

        public RequestPipeline(IItemProvider itemProvider)
        {
            _itemProvider = itemProvider;

            Add(new FilterRequestProcessor());
            Add(new CreateContextProcessor());
            Add(new ResolveLanguageProcessor());
            Add(new ResolveItemProcessor());
        }

        public PipelineArgs GetArgs(HttpContext context)
        {
            return new RequestArgs(context, _itemProvider);
        }
    }
}