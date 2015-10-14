using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Lightcore.Kernel.Pipelines
{
    public abstract class Pipeline<T> where T: PipelineArgs
    {
        private IEnumerable<Processor<T>> _processors;
        public bool IsEnded { get; private set; }

        public abstract IEnumerable<Processor<T>> GetProcessors();

        public virtual void Run(T args)
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

        private IEnumerable<Processor<T>> GetCoreProcessors()
        {
            return _processors ?? (_processors = GetProcessors());
        }

        public virtual async Task RunAsync(T args)
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