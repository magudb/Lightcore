using System.IO;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Urls;

namespace Lightcore.Kernel.Pipelines.RenderField
{
    public class RenderFieldArgs : PipelineArgs
    {
        public RenderFieldArgs(IItemProvider itemProvider, IItemUrlService itemUrlService, Item item, Field field, TextWriter output)
        {
            ItemProvider = itemProvider;
            ItemUrlService = itemUrlService;
            Item = item;
            Field = field;
            Output = output;
        }

        public IItemProvider ItemProvider { get; }
        public IItemUrlService ItemUrlService { get; }
        public Item Item { get; }
        public Field Field { get; }
        public TextWriter Output { get; }
    }
}