using System;
using System.Collections.Generic;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Pipelines.RenderPlaceholder.Processors;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;

namespace Lightcore.Kernel.Pipelines.RenderPlaceholder
{
    public class RenderPlaceholderPipeline : Pipeline<RenderPlaceholderArgs>
    {
        public override IEnumerable<Processor<RenderPlaceholderArgs>> GetProcessors()
        {
            yield return new RenderPlaceholderProcessor();
        }

        public RenderPlaceholderArgs GetArgs(HttpContext httpContext, RouteData routeData, Item item, string name)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (name == null)
            {
                throw new ArgumentException("Was empty", nameof(name));
            }

            return new RenderPlaceholderArgs(httpContext, routeData, item, name);
        }
    }
}