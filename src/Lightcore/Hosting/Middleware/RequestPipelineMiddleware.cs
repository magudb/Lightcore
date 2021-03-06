using System.Threading.Tasks;
using Lightcore.Kernel.Pipelines.Request;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace Lightcore.Hosting.Middleware
{
    public class RequestPipelineMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RequestPipeline _pipeline;

        public RequestPipelineMiddleware(RequestDelegate next, RequestPipeline pipeline)
        {
            _next = next;
            _pipeline = pipeline;
        }

        public async Task Invoke(HttpContext context)
        {
            await _pipeline.RunAsync(_pipeline.GetArgs(context));

            if (!_pipeline.IsEnded)
            {
                await _next(context);
            }
        }
    }
}