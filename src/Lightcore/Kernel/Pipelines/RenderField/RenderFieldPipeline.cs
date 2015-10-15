using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Pipelines.RenderField.Processors;
using Lightcore.Kernel.Urls;

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

        public override Task RunAsync(RenderFieldArgs args)
        {
            if (args.Field == null)
            {
                args.AbortPipeline();

                return Task.FromResult(0);
            }

            return base.RunAsync(args);
        }

        public RenderFieldArgs GetArgs(Item item, Field field, TextWriter writer)
        {
            Requires.IsNotNull(item, nameof(item));
            Requires.IsNotNull(writer, nameof(writer));

            return new RenderFieldArgs(_itemProvider, _itemUrlService, item, field, writer);
        }
    }
}