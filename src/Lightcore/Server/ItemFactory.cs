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
        public static Item Create(ServerResponseModel apiResponse, Language language)
        {
            var data = MapItemDefinition(apiResponse, language);

            data.Children = apiResponse.Children.Select(child => MapItemDefinition(child, language));

            var item = new Item(data);

            return item;
        }

        private static ItemDefinition MapItemDefinition(ServerResponseModel apiResponse, Language language)
        {
            var item = new ItemDefinition
            {
                Language = language,
                Id = apiResponse.Item.Id,
                Key = apiResponse.Item.Name.ToLowerInvariant(),
                Name = apiResponse.Item.Name,
                Path = apiResponse.Item.FullPath,
                HasVersion = apiResponse.Item.HasVersion,
                TemplateId = apiResponse.Item.TemplateId,
                ParentId = apiResponse.Item.ParentId,
                Fields = new FieldCollection(apiResponse.Fields.Select(MapField))
            };

            if (apiResponse.Presentation != null)
            {
                item.Visualization = new PresentationDetails(new Layout(apiResponse.Presentation.Layout.Path), MapRenderings(apiResponse.Presentation));
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