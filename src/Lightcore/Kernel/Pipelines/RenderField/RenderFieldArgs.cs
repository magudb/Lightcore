using Lightcore.Kernel.Data;
using Lightcore.Kernel.Urls;
using Microsoft.AspNet.Mvc.Rendering;

namespace Lightcore.Kernel.Pipelines.RenderField
{
    public class RenderFieldArgs : PipelineArgs
    {
        public RenderFieldArgs(IItemProvider itemProvider, IItemUrlService itemUrlService, Item item, Field field)
        {
            ItemProvider = itemProvider;
            ItemUrlService = itemUrlService;
            Item = item;
            Field = field;
            Results = HtmlString.Empty;
            Raw = string.Empty;
        }

        public IItemProvider ItemProvider { get; }
        public IItemUrlService ItemUrlService { get; }
        public Item Item { get; }
        public Field Field { get; }

        public HtmlString Results { get; set; }
        public string Raw { get; set; }
    }
}