using System;
using System.Collections.Generic;
using System.Linq;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Data.Fields;
using Lightcore.Kernel.Data.Globalization;
using Lightcore.Kernel.Data.Presentation;
using Lightcore.Server.Models;
using Newtonsoft.Json.Linq;

namespace Lightcore.Server
{
    internal static class ItemFactory
    {
        public static IEnumerable<Item> Create(ServerResponseModel apiResponse)
        {
            return (from apiItem in apiResponse.Items
                    let itemDefinition = MapItemDefinition(apiItem)
                    select MapItem(itemDefinition, apiItem)).ToList();
        }

        private static ItemDefinition MapItemDefinition(ItemModel apiItem)
        {
            return new ItemDefinition(apiItem.Properties.Id,
                apiItem.Properties.TemplateId,
                apiItem.Properties.Name,
                apiItem.Properties.FullPath,
                Language.Parse(apiItem.Properties.Language),
                apiItem.Properties.HasVersion,
                new FieldCollection(apiItem.Fields.Select(MapField)));
        }

        private static Item MapItem(ItemDefinition itemDefinition, ItemModel apiItem)
        {
            Item item;

            if (apiItem.Presentation != null)
            {
                item = new Item(itemDefinition, apiItem.Properties.ParentId,
                    apiItem.Children.Select(MapItemDefinition),
                    new PresentationDetails(new Layout(apiItem.Presentation.Layout.Path), MapRenderings(apiItem.Presentation)));
            }
            else
            {
                item = new Item(itemDefinition, apiItem.Properties.ParentId, apiItem.Children.Select(MapItemDefinition));
            }

            return item;
        }

        private static IEnumerable<Rendering> MapRenderings(PresentationModel apiPresentation)
        {
            return apiPresentation.Renderings.Select(r =>
                new Rendering(r.Placeholder, r.Datasource, r.Controller, r.Action, r.Parameters,
                    new Caching(r.Caching.Cacheable, r.Caching.VaryByItem, r.Caching.VaryByParm, r.Caching.VaryByQueryString)));
        }

        private static Field MapField(FieldModel apiField)
        {
            if (apiField.Type.Equals("image"))
            {
                var value = (JObject)apiField.Value;

                return new ImageField(apiField.Key,
                    apiField.Type,
                    apiField.Id,
                    value["Alt"].Value<string>(),
                    value["Url"].Value<string>());
            }

            if (apiField.Type.Equals("general link"))
            {
                var value = (JObject)apiField.Value;

                return new LinkField(apiField.Key,
                    apiField.Type,
                    apiField.Id, value["Description"].Value<string>(),
                    value["TargetUrl"].Value<string>(),
                    Guid.Parse(value["TargetId"].Value<string>()));
            }

            return new Field(apiField.Key, apiField.Key, apiField.Id, apiField.Value as string);
        }
    }
}