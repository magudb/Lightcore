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
            var items = new List<Item>();

            foreach (var model in apiResponse.Items)
            {
                var data = MapItemDefinition(model);

                data.Children = model.Children.Select(child => MapItemDefinition(child));

                items.Add(new Item(data));
            }

            return items;
        }

        private static ItemDefinition MapItemDefinition(ItemModel apiItem)
        {
            var item = new ItemDefinition
            {
                Language = Language.Parse(apiItem.Properties.Language),
                Id = apiItem.Properties.Id,
                Key = apiItem.Properties.Name.ToLowerInvariant(),
                Name = apiItem.Properties.Name,
                Path = apiItem.Properties.FullPath,
                HasVersion = apiItem.Properties.HasVersion,
                TemplateId = apiItem.Properties.TemplateId,
                ParentId = apiItem.Properties.ParentId,
                Fields = new FieldCollection(apiItem.Fields.Select(MapField))
            };

            if (apiItem.Presentation != null)
            {
                item.Visualization = new PresentationDetails(new Layout(apiItem.Presentation.Layout.Path), MapRenderings(apiItem.Presentation));
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