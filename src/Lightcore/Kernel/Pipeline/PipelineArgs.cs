using Microsoft.AspNet.Http;

namespace Lightcore.Kernel.Pipeline
{
    public class PipelineArgs
    {
        public PipelineArgs(HttpContext httpContext)
        {
            Context = httpContext;
        }

        public HttpContext Context { get; }
        public bool IsAborted { get; private set; }

        public void Abort()
        {
            IsAborted = true;
        }
    }
}