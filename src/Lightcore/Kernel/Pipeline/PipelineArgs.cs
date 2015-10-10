namespace Lightcore.Kernel.Pipeline
{
    public class PipelineArgs
    {
        public bool IsAborted { get; private set; }

        public void Abort()
        {
            IsAborted = true;
        }
    }
}