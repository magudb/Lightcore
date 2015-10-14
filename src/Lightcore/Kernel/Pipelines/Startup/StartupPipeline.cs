using System.Collections.Generic;
using Lightcore.Kernel.Configuration;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Pipelines.Startup.Processors;
using Microsoft.AspNet.Http;
using Microsoft.Framework.OptionsModel;

namespace Lightcore.Kernel.Pipelines.Startup
{
    public class StartupPipeline : Pipeline
    {
        private readonly IItemProvider _itemProvider;
        private readonly LightcoreOptions _options;

        private readonly object _startupLock = new object();
        private bool _isStarted;

        public StartupPipeline(IItemProvider itemProvider, IOptions<LightcoreOptions> options)
        {
            _itemProvider = itemProvider;
            _options = options.Options;
        }

        public override IEnumerable<Processor> GetProcessors()
        {
            yield return new VerifyConfigProcessor();
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

        public StartupArgs GetArgs(HttpContext context)
        {
            return new StartupArgs(context, _itemProvider, _options);
        }
    }
}