using System.Collections.Generic;
using System.IO;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Data.Fields;
using Lightcore.Kernel.Url;

namespace Lightcore.Kernel.Pipelines.RenderField
{
    public class RenderFieldArgs : PipelineArgs
    {
        public RenderFieldArgs(IItemProvider itemProvider, IItemUrlService itemUrlService, Item item, Field field, TextWriter output,
                               Dictionary<string, string> attributes)
        {
            ItemProvider = itemProvider;
            ItemUrlService = itemUrlService;
            Item = item;
            Field = field;
            Output = output;
            Attributes = attributes;
        }

        public IItemProvider ItemProvider { get; }
        public IItemUrlService ItemUrlService { get; }
        public Item Item { get; }
        public Field Field { get; }
        public TextWriter Output { get; }
        public Dictionary<string, string> Attributes { get; }
    }
}