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
                TemplateName = apiItem.TemplateName,
                ParentId = apiItem.ParentId,
                Fields = new FieldCollection(apiFields.Select(MapField))
            };

            if (apiPresentation != null)
            {
                item.Visualization = new ItemVisualization
                {
                    Layout = new Layout
                    {
                        Path = apiPresentation.Layout.Path
                    },
                    Renderings = apiPresentation.Renderings
                                                .Select(r => new Rendering(r.Placeholder, r.DataSource, r.Controller, r.Action, r.Parameters))
                };
            }

            return item;
        }

        private static Field MapField(FieldModel apiField)
        {
            if (apiField.Type.Equals("image"))
            {
                var field = new ImageField
                {
                    Id = apiField.Id,
                    Type = apiField.Type,
                    Key = apiField.Key
                };

                var value = (JObject)apiField.Value;

                field.Alt = value["Alt"].Value<string>();
                field.Url = value["Url"].Value<string>();

                return field;
            }

            if (apiField.Type.Equals("general link"))
            {
                var field = new LinkField
                {
                    Id = apiField.Id,
                    Type = apiField.Type,
                    Key = apiField.Key
                };

                var value = (JObject)apiField.Value;

                field.TargetId = Guid.Parse(value["TargetId"].Value<string>());
                field.TargetUrl = value["TargetUrl"].Value<string>();
                field.Description = value["Description"].Value<string>();

                return field;
            }
            else
            {
                var field = new Field
                {
                    Id = apiField.Id,
                    Type = apiField.Type,
                    Key = apiField.Key,
                    Value = apiField.Value as string
                };

                return field;
            }
        }
    }
}