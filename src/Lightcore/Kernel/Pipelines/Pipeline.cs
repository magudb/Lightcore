using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Lightcore.Kernel.Pipelines
{
    public abstract class Pipeline
    {
        private IEnumerable<Processor> _processors;
        public bool IsEnded { get; private set; }

        public abstract IEnumerable<Processor> GetProcessors();

        public virtual void Run(PipelineArgs args)
        {
            foreach (var processor in GetCoreProcessors())
            {
                if (args.IsEnded || args.IsAborted)
                {
                    break;
                }

                Debug.WriteLine("{0}: Running... ", new object[] {processor.GetType().Name});

                processor.Process(args);

                Debug.WriteLine("{0}: aborted:{1}, ended:{2}", processor.GetType().Name, args.IsAborted, args.IsEnded);
            }

            IsEnded = args.IsEnded;
        }

        private IEnumerable<Processor> GetCoreProcessors()
        {
            return _processors ?? (_processors = GetProcessors());
        }

        public virtual async Task RunAsync(PipelineArgs args)
        {
            foreach (var processor in GetCoreProcessors())
            {
                if (args.IsEnded || args.IsAborted)
                {
                    break;
                }

                Debug.WriteLine("{0}: Running async... ", new object[] {processor.GetType().Name});

                await processor.ProcessAsync(args);

                Debug.WriteLine("{0}: aborted:{1}, ended:{2}", processor.GetType().Name, args.IsAborted, args.IsEnded);
            }

            IsEnded = args.IsEnded;
        }
    }
}