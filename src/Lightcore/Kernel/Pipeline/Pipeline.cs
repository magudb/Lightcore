using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lightcore.Kernel.Pipeline
{
    public class Pipeline
    {
        private readonly List<Processor> _processors = new List<Processor>();
        public bool IsAborted { get; private set; }

        public Pipeline Add(Processor processor)
        {
            _processors.Add(processor);

            return this;
        }

        public async Task RunAsync(PipelineArgs args)
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
    }
}