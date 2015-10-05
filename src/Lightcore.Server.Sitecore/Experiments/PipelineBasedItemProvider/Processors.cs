using System.Diagnostics;
using Sitecore.Pipelines.ItemProvider.CreateItem;
using Sitecore.Pipelines.ItemProvider.DeleteItem;
using Sitecore.Pipelines.ItemProvider.SaveItem;

namespace Lightcore.Server.Sitecore.Experiments.PipelineBasedItemProvider
{
    public class SaveItemProcessor : global::Sitecore.Pipelines.ItemProvider.SaveItem.SaveItemProcessor
    {
        public override void Process(SaveItemArgs args)
        {
            Debug.WriteLine("SaveItemProcessor: " + args.Item.Paths.FullPath + ", " + args.Item.Database.Name);
        }
    }

    public class CreateItemProcessor : global::Sitecore.Pipelines.ItemProvider.CreateItem.CreateItemProcessor
    {
        public override void Process(CreateItemArgs args)
        {
            Debug.WriteLine("CreateItemProcessor: " + args.ItemName + ", " + args.Destination.Database.Name);
        }
    }

    public class DeleteItemProcessor : global::Sitecore.Pipelines.ItemProvider.DeleteItem.DeleteItemProcessor
    {
        public override void Process(DeleteItemArgs args)
        {
            Debug.WriteLine("DeleteItemProcessor: " + args.Item.Paths.FullPath + ", " + args.Item.Database.Name);
        }
    }
}