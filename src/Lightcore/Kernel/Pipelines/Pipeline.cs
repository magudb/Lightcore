using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;

namespace Lightcore.Kernel.Pipelines
{
    public abstract class Pipeline
    {
        private readonly List<Processor> _processors = new List<Processor>();
        public bool IsAborted { get; private set; }

        public Pipeline Add(Processor processor)
        {
            _processors.Add(processor);

            return this;
        }

        public virtual void Run(PipelineArgs args)
        {
            foreach (var processor in _processors)
            {
                if (!args.IsAborted)
                {
                    processor.Process(args);
                }
            }

            IsAborted = args.IsAborted;
        }

        public virtual async Task RunAsync(PipelineArgs args)
        {
            foreach (var processor in _processors)
            {
                if (!args.IsAborted)
                {
                    await processor.ProcessAsync(args);
                }
            }

            IsAborted = args.IsAborted;
        }

        public abstract PipelineArgs GetArgs(HttpContext context);
    }
}