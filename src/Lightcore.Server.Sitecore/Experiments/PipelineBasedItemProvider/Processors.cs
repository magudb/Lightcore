using System.Diagnostics;
using System.IO;
using Sitecore.Data.Items;
using Sitecore.Pipelines.ItemProvider.DeleteItem;
using Sitecore.Pipelines.ItemProvider.SaveItem;
using ItemSerializer = Lightcore.Server.Sitecore.Data.ItemSerializer;

namespace Lightcore.Server.Sitecore.Experiments.PipelineBasedItemProvider
{
    public class SaveItemProcessor : global::Sitecore.Pipelines.ItemProvider.SaveItem.SaveItemProcessor
    {
        public override void Process(SaveItemArgs args)
        {
            new ItemDiskStore().Save(args.Item);

            Debug.WriteLine("SaveItemProcessor: " + args.Item.Paths.FullPath + ", " + args.Item.Database.Name);
        }
    }

    public class DeleteItemProcessor : global::Sitecore.Pipelines.ItemProvider.DeleteItem.DeleteItemProcessor
    {
        public override void Process(DeleteItemArgs args)
        {
            new ItemDiskStore().Delete(args.Item);

            Debug.WriteLine("DeleteItemProcessor: " + args.Item.Paths.FullPath + ", " + args.Item.Database.Name);
        }
    }

    internal class ItemDiskStore
    {
        private readonly ItemSerializer _itemConverter;

        public ItemDiskStore()
        {
            _itemConverter = new ItemSerializer();
        }

        private string GetIdFilePath(Item item, string device)
        {
            var idPath = item.ID.Guid.ToString().Replace('-', '\\');

            return string.Format("e:\\temp\\Lightcore\\{0}\\{1}\\{2}\\{3}", item.Database.Name, item.Language.Name, device, idPath);
        }

        private string GetFilePath(Item item, string device)
        {
            var path = item.Paths.FullPath;

            return string.Format("e:\\temp\\Lightcore\\{0}\\{1}\\{2}\\{3}", item.Database.Name, item.Language.Name, device, path);
        }

        public void Save(Item item)
        {
            if (!item.Database.Name.Equals("web") || !item.Paths.IsContentItem)
            {
                return;
            }

            var device = "default";
            var path = GetIdFilePath(item, device);

            Directory.CreateDirectory(path);

            using (var outputStream = File.Open(path + "\\item.json", FileMode.Create, FileAccess.Write))
            {
                _itemConverter.SerializeItem(item, outputStream, "default");
            }

            path = GetFilePath(item, device);

            Directory.CreateDirectory(path);

            using (var outputStream = File.Open(path + "\\item.json", FileMode.Create, FileAccess.Write))
            {
                _itemConverter.SerializeItem(item, outputStream, "default");
            }
        }

        public void Delete(Item item)
        {
            if (!item.Database.Name.Equals("web") || !item.Paths.IsContentItem)
            {
                return;
            }

            var device = "default";
            var path = GetIdFilePath(item, device);

            File.Delete(path + "\\item.json");
        }
    }
}