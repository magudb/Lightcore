namespace Lightcore.Kernel.Pipelines.Startup.Processors
{
    public class VerifyConfigProcessor : Processor<StartupArgs>
    {
        public override void Process(StartupArgs args)
        {
            args.Options.Verify();
        }
    }
}