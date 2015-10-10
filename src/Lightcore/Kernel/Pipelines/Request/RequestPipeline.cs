using Lightcore.Kernel.Configuration;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Pipelines.Request.Processors;
using Microsoft.AspNet.Http;
using Microsoft.Framework.OptionsModel;

namespace Lightcore.Kernel.Pipelines.Request
{
    public class RequestPipeline : Pipeline
    {
        private readonly LightcoreOptions _options;
        private readonly IItemProvider _itemProvider;

        public RequestPipeline(IItemProvider itemProvider, IOptions<LightcoreOptions> options)
        {
            _itemProvider = itemProvider;
            _options = options.Options;

            Add(new FilterRequestProcessor());
            Add(new CreateContextProcessor());
            Add(new ResolveLanguageProcessor());
            Add(new ResolveContentPathProcessor());
            Add(new ResolveItemProcessor());
            Add(new VerifyLayoutProcessor());
        }

        public override PipelineArgs GetArgs(HttpContext context)
        {
            return new RequestArgs(context, _itemProvider, _options);
        }
    }
}