using System.Collections.Generic;
using System.Linq;
using Lightcore.Kernel.Data;
using Lightcore.Server.Models;

namespace Lightcore.Server
{
    public static class ServerResponseModelToItemMapper
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
                TemplateId =  apiItem.TemplateId,
                TemplateName = apiItem.TemplateName,
                ParentId = apiItem.ParentId,
                Fields = new FieldCollection(apiFields.Select(f => new Field
                {
                    Key = f.Key,
                    Type = f.Type,
                    Value = f.Value,
                    Id = f.Id
                }))
            };

            if (apiPresentation != null)
            {
                item.Visualization = new ItemVisualization
                {
                    Layout = new Layout
                    {
                        Path = apiPresentation.Layout.Path
                    },
                    Renderings = apiPresentation.Renderings.Select(r => new Rendering(r.Placeholder, r.DataSource, r.Controller, r.Action, r.Parameters))
                };
            }

            return item;
        }
    }
}