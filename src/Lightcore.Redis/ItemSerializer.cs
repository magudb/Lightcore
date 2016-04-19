using System;
using System.Collections.Generic;
using System.Linq;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Data.Fields;
using Lightcore.Kernel.Data.Globalization;
using Lightcore.Kernel.Data.Presentation;
using Lightcore.Redis.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lightcore.Redis
{
    internal static class ItemSerializer
    {
        public static IEnumerable<Item> Deserialize(string data)
        {
            var model = JsonConvert.DeserializeObject<ItemModel>(data);
            var itemDefinition = MapItemDefinition(model);
            var item = MapItem(itemDefinition, model);

            return new[] {item};
        }

        private static ItemDefinition MapItemDefinition(ItemModel itemModel)
        {
            return new ItemDefinition(itemModel.Properties.Id,
                itemModel.Properties.TemplateId,
                itemModel.Properties.Name,
                itemModel.Properties.FullPath,
                Language.Parse(itemModel.Properties.Language),
                itemModel.Properties.HasVersion,
                new FieldCollection(itemModel.Fields.Select(MapField)));
        }

        private static Item MapItem(ItemDefinition itemDefinition, ItemModel itemModel)
        {
            Item item;

            if (itemModel.Presentation != null)
            {
                item = new Item(itemDefinition,
                    itemModel.Properties.ParentId,
                    itemModel.Children.Select(MapItemDefinition),
                    new PresentationDetails(new Layout(itemModel.Presentation.Layout.Path),
                        MapRenderings(itemModel.Presentation)));
            }
            else
            {
                item = new Item(itemDefinition, itemModel.Properties.ParentId, itemModel.Children.Select(MapItemDefinition));
            }

            return item;
        }

        private static IEnumerable<Rendering> MapRenderings(PresentationModel presentationModel)
        {
            return presentationModel.Renderings.Select(r =>
                new Rendering(r.Placeholder, r.Datasource, r.Controller, r.Action, r.Parameters,
                    new Caching(r.Caching.Cacheable, r.Caching.VaryByItem, r.Caching.VaryByParameter, r.Caching.VaryByQueryString)));
        }

        private static Field MapField(FieldModel fieldModel)
        {
            if (fieldModel.Type.Equals("image"))
            {
                var value = (JObject)fieldModel.Value;

                return new ImageField(fieldModel.Key,
                    fieldModel.Type,
                    fieldModel.Id,
                    value["Alt"].Value<string>(),
                    value["Url"].Value<string>());
            }

            if (fieldModel.Type.Equals("general link"))
            {
                var value = (JObject)fieldModel.Value;

                return new LinkField(fieldModel.Key,
                    fieldModel.Type,
                    fieldModel.Id, value["Description"].Value<string>(),
                    value["TargetUrl"].Value<string>(),
                    Guid.Parse(value["TargetId"].Value<string>()));
            }

            return new Field(fieldModel.Key, fieldModel.Key, fieldModel.Id, fieldModel.Value as string);
        }
    }
}