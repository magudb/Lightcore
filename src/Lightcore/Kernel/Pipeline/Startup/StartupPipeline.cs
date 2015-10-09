using Lightcore.Kernel.Configuration;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Pipeline.Startup.Processor;
using Microsoft.AspNet.Http;
using Microsoft.Framework.OptionsModel;

namespace Lightcore.Kernel.Pipeline.Startup
{
    public class StartupPipeline : Pipeline
    {
        private readonly IOptions<LightcoreConfig> _config;
        private readonly IItemProvider _itemProvider;

        private readonly object _startupLock = new object();
        private bool _isStarted;

        public StartupPipeline(IItemProvider itemProvider, IOptions<LightcoreConfig> config)
        {
            _itemProvider = itemProvider;
            _config = config;

            Add(new SetupSupportedLanguagesProcessor());
        }

        public override void Run(PipelineArgs args)
        {
            if (_isStarted == false)
            {
                lock (_startupLock)
                {
                    if (_isStarted == false)
                    {
                        base.Run(args);

                        _isStarted = true;
                    }
                }
            }
        }

        public PipelineArgs GetArgs(HttpContext context)
        {
            return new StartupArgs(context, _itemProvider, _config.Options);
        }
    }
}