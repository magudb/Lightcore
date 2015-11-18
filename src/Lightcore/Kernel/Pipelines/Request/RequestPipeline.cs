using System.Collections.Generic;
using Lightcore.Configuration;
using Lightcore.Kernel.Data.Providers;
using Lightcore.Kernel.Pipelines.Request.Processors;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.OptionsModel;

namespace Lightcore.Kernel.Pipelines.Request
{
    public class RequestPipeline : Pipeline<RequestArgs>
    {
        private readonly IItemProvider _itemProvider;
        private readonly LightcoreOptions _options;

        public RequestPipeline(IItemProvider itemProvider, IOptions<LightcoreOptions> options)
        {
            _itemProvider = itemProvider;
            _options = options.Value;
        }

        public RequestArgs GetArgs(HttpContext context)
        {
            return new RequestArgs(context, _itemProvider, _options);
        }

        public override IEnumerable<Processor<RequestArgs>> GetProcessors()
        {
            yield return new FilterRequestProcessor();
            yield return new CreateContextProcessor();
            yield return new ResolveLanguageProcessor();
            yield return new ResolveContentPathProcessor();
            yield return new ResolveItemProcessor();
            yield return new VerifyLayoutProcessor();
        }
    }
}