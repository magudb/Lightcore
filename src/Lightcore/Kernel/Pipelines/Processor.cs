using System.Threading.Tasks;

namespace Lightcore.Kernel.Pipelines
{
    public abstract class Processor<T> where T : PipelineArgs
    {
        public virtual Task ProcessAsync(T args)
        {
            return Task.Run(() => Process(args));
        }

        public virtual void Process(T args)
        {
        }
    }
}