namespace Lightcore.Kernel.Pipelines.Startup.Processors
{
    public class VerifyConfigProcessor : Processor<StartupArgs>
    {
        public override void Process(StartupArgs args)
        {
            var startupArgs = (StartupArgs)args;

            startupArgs.Options.Verify();
        }
    }
}