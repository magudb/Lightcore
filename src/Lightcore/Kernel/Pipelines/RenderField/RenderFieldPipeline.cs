using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Data.Fields;
using Lightcore.Kernel.Data.Providers;
using Lightcore.Kernel.Pipelines.RenderField.Processors;
using Lightcore.Kernel.Url;

namespace Lightcore.Kernel.Pipelines.RenderField
{
    public class RenderFieldPipeline : Pipeline<RenderFieldArgs>
    {
        private readonly IItemProvider _itemProvider;
        private readonly IItemUrlService _itemUrlService;

        public RenderFieldPipeline(IItemProvider itemProvider, IItemUrlService itemUrlService)
        {
            _itemProvider = itemProvider;
            _itemUrlService = itemUrlService;
        }

        public override IEnumerable<Processor<RenderFieldArgs>> GetProcessors()
        {
            yield return new RenderLinkFieldProcessor();
            yield return new RenderMediaFieldProcessor();
            yield return new RenderFieldProcessor();
        }

        public override async Task RunAsync(RenderFieldArgs args)
        {
            if (args.Field == null)
            {
                args.AbortPipeline();

                return;
            }

            await base.RunAsync(args);
        }

        public RenderFieldArgs GetArgs(Item item, Field field, TextWriter output, Dictionary<string, string> attributes)
        {
            Requires.IsNotNull(item, nameof(item));
            Requires.IsNotNull(output, nameof(output));

            return new RenderFieldArgs(_itemProvider, _itemUrlService, item, field, output, attributes);
        }
    }
}