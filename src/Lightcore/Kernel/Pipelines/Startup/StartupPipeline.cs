using System.Collections.Generic;
using System.Threading.Tasks;
using Lightcore.Kernel.Configuration;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Pipelines.Startup.Processors;
using Microsoft.AspNet.Http;
using Microsoft.Framework.OptionsModel;

namespace Lightcore.Kernel.Pipelines.Startup
{
    public class StartupPipeline : Pipeline<StartupArgs>
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

        public override IEnumerable<Processor<StartupArgs>> GetProcessors()
        {
            yield return new VerifyConfigProcessor();
        }

        public override Task RunAsync(StartupArgs args)
        {
            if (_isStarted == false)
            {
                lock (_startupLock)
                {
                    if (_isStarted == false)
                    {
                        base.RunAsync(args).Wait();

                        _isStarted = true;
                    }
                }
            }

            return Task.FromResult(0);
        }

        public StartupArgs GetArgs(HttpContext context)
        {
            return new StartupArgs(context, _itemProvider, _options);
        }
    }
}