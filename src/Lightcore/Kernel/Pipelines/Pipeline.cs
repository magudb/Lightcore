using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;

namespace Lightcore.Kernel.Pipelines
{
    public abstract class Pipeline
    {
        private readonly List<Processor> _processors = new List<Processor>();
        public bool IsEnded { get; private set; }

        public Pipeline Add(Processor processor)
        {
            _processors.Add(processor);

            return this;
        }

        public virtual void Run(PipelineArgs args)
        {
            foreach (var processor in _processors)
            {
                if (args.IsEnded || args.IsAborted)
                {
                    break;
                }

                processor.Process(args);
            }

            IsEnded = args.IsEnded;
        }

        public virtual async Task RunAsync(PipelineArgs args)
        {
            foreach (var processor in _processors)
            {
                if (args.IsEnded || args.IsAborted)
                {
                    break;
                }

                Debug.WriteLine("{0}: Running... ", new object[] {processor.GetType().Name});

                await processor.ProcessAsync(args);

                Debug.WriteLine("{0}: aborted:{1}, ended:{2}", processor.GetType().Name, args.IsAborted, args.IsEnded);
            }

            IsEnded = args.IsEnded;
        }

        public abstract PipelineArgs GetArgs(HttpContext context);
    }
}