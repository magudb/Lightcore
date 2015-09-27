using Lightcore.Kernel.Data;
using Microsoft.AspNet.Http;

namespace Lightcore.Kernel.Pipeline
{
    public class PipelineArgs
    {
        public PipelineArgs(HttpContext httpContext, IItemProvider itemProvider)
        {
            Context = httpContext;
            ItemProvider = itemProvider;
        }

        public IItemProvider ItemProvider { get; }
        public HttpContext Context { get; }
        public bool IsAborted { get; private set; }

        public void Abort()
        {
            IsAborted = true;
        }
    }
}