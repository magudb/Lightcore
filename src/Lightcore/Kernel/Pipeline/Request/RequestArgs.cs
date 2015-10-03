using Lightcore.Kernel.Data;
using Microsoft.AspNet.Http;

namespace Lightcore.Kernel.Pipeline.Request
{
    public class RequestArgs : PipelineArgs
    {
        public RequestArgs(HttpContext httpContext, IItemProvider itemProvider) : base(httpContext)
        {
            ItemProvider = itemProvider;
        }

        public IItemProvider ItemProvider { get; }
    }
}