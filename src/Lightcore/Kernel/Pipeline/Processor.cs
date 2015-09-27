using System.Threading.Tasks;

namespace Lightcore.Kernel.Pipeline
{
    public abstract class Processor
    {
        public virtual Task ProcessAsync(PipelineArgs args)
        {
            return Task.Run(() => Process(args));
        }

        public virtual void Process(PipelineArgs args)
        {
        }
    }
}