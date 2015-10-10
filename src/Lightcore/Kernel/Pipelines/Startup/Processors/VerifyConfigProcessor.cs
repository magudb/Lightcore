namespace Lightcore.Kernel.Pipelines.Startup.Processors
{
    public class VerifyConfigProcessor : Processor
    {
        public override void Process(PipelineArgs args)
        {
            var startupArgs = (StartupArgs)args;

            startupArgs.Options.Verify();
        }
    }
}