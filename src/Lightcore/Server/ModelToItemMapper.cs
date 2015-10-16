using System;
using System.Collections.Generic;
using System.Linq;
using Lightcore.Kernel.Data;
using Lightcore.Server.Models;
using Newtonsoft.Json.Linq;

namespace Lightcore.Server
{
    public static class ModelToItemMapper
    {
        public static Item Map(ServerResponseModel apiResponse, Language language)
        {
            var item = MapItem(apiResponse.Item, apiResponse.Fields, apiResponse.Presentation, language);

            item.Children = apiResponse.Children.Select(child => MapItem(child.Item, child.Fields, child.Presentation, language));

            return item;
        }

        private static Item MapItem(ItemModel apiItem, IEnumerable<FieldModel> apiFields, PresentationModel apiPresentation, Language language)
        {
            var item = new Item
            {
                Language = language,
                Id = apiItem.Id,
                Key = apiItem.Name.ToLowerInvariant(),
                Name = apiItem.Name,
                Path = apiItem.FullPath,
                HasVersion = apiItem.HasVersion,
                TemplateId = apiItem.TemplateId,
                ParentId = apiItem.ParentId,
                Fields = new FieldCollection(apiFields.Select(MapField))
            };

            if (apiPresentation != null)
            {
                item.Visualization = new Presentation(new Layout(apiPresentation.Layout.Path), MapRenderings(apiPresentation));
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