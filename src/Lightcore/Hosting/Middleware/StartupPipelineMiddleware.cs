using System.Threading.Tasks;
using Lightcore.Kernel.Pipelines.Startup;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace Lightcore.Hosting.Middleware
{
    public class StartupPipelineMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly StartupPipeline _pipeline;

        public StartupPipelineMiddleware(RequestDelegate next, StartupPipeline pipeline)
        {
            _next = next;
            _pipeline = pipeline;
        }

        public async Task Invoke(HttpContext context)
        {
            _pipeline.Run(_pipeline.GetArgs(context));

            if (!_pipeline.IsEnded)
            {
                await _next(context);
            }
        }
    }
}