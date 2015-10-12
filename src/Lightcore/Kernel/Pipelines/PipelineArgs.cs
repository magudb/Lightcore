namespace Lightcore.Kernel.Pipelines
{
    public class PipelineArgs
    {
        internal bool IsEnded { get; private set; }

        internal bool IsAborted { get; private set; }

        public void EndPipeline()
        {
            IsEnded = true;
        }

        public void AbortPipeline()
        {
            IsAborted = true;
        }
    }
}