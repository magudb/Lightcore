namespace Lightcore.Kernel.Pipelines
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