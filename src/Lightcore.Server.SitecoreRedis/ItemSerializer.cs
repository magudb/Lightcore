using System;
using System.Collections.Generic;
using System.Linq;
using Lightcore.Redis.Models;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;

namespace Lightcore.Server.SitecoreRedis
{
    internal class ItemSerializer
    {
        private readonly ID _controllerRenderingTemplateId = ID.Parse("{2A3E91A0-7987-44B5-AB34-35C2D9DE83B9}");

        public string Serialize(Item item, string[] additionalFields)
        {
            var model = new ItemModel
            {
                Properties = MapItem(item),
                Fields = MapFields(item, additionalFields),
                Presentation = MapPresentation(item),
                Children = item.GetChildren().Select(child => new ItemModel
                {
                    Properties = MapItem(child),
                    Fields = MapFields(child, additionalFields)
                })
            };

            return JsonConvert.SerializeObject(model);
        }

        private ItemPropertyModel MapItem(Item item)
        {
            return new ItemPropertyModel
            {
                Id = item.ID.Guid,
                FullPath = item.Paths.FullPath.ToLowerInvariant(),
                Language = item.Language.Name,
                Name = item.Name,
                ParentId = item.ParentID.Guid,
                TemplateId = item.TemplateID.Guid,
                HasVersion = item.Versions.Count > 0
            };
        }

        private PresentationModel MapPresentation(Item item, string deviceName = "default")
        {
            var deviceItem = item.Database.Resources.Devices["/sitecore/layout/devices/" + deviceName];

            if (deviceItem == null)
            {
                return null;
            }

            var layout = item.Visualization.GetLayout(deviceItem);

            if (layout == null)
            {
                return null;
            }

            var presentation = new PresentationModel
            {
                Layout = new LayoutModel
                {
                    Path = layout.FilePath
                }
            };

            var controllerRenderings = item.Visualization.GetRenderings(deviceItem, false)
                                           .Where(r => r.RenderingItem.InnerItem.TemplateID == _controllerRenderingTemplateId);

            presentation.Renderings = controllerRenderings.Select(rendering =>
            {
                var action = rendering.RenderingItem.InnerItem["Action"];
                var datasource = rendering.Settings.DataSource;
                var parameters = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(rendering.Settings.Parameters))
                {
                    parameters = rendering.Settings.Parameters.Split('&').Select(p => p.Split('=')).ToDictionary(param => param[0], param => param[1]);
                }

                return new RenderingModel
                {
                    Placeholder = rendering.Placeholder,
                    Controller = rendering.RenderingItem.InnerItem["Controller"],
                    Action = !string.IsNullOrEmpty(action) ? action : "Index",
                    Datasource = !string.IsNullOrEmpty(datasource) ? datasource : item.ID.Guid.ToString(),
                    Parameters = parameters,
                    Caching = new RenderingCachingModel
                    {
                        Cacheable = rendering.Settings.Caching.Cacheable,
                        VaryByItem = rendering.Settings.Caching.VaryByData,
                        VaryByParameter = rendering.Settings.Caching.VaryByParm,
                        VaryByQueryString = rendering.Settings.Caching.VaryByQueryString
                    }
                };
            });

            return presentation;
        }

        private IEnumerable<FieldModel> MapFields(Item item, string[] additionalFields)
        {
            var fields = new List<FieldModel>(item.Fields.Where(f => !f.Key.StartsWith("__")).Select(MapField).Where(f => f != null));

            foreach (var key in additionalFields)
            {
                var field = item.Fields[key.Trim()];

                if (field != null)
                {
                    fields.Add(MapField(field));
                }
            }

            return fields;
        }

        private FieldModel MapField(Field field)
        {
            object value;

            if (field.TypeKey.Equals("image"))
            {
                var media = (ImageField)field;

                if (media.MediaItem == null)
                {
                    return null;
                }

                value = new ImageFieldValueModel
                {
                    Alt = media.Alt,
                    Url = MediaManager.GetMediaUrl(media.MediaItem, new MediaUrlOptions
                    {
                        MediaLinkServerUrl = null,
                        AlwaysIncludeServerUrl = true,
                        IncludeExtension = true,
                        LowercaseUrls = true,
                        UseItemPath = true
                    })
                };
            }
            else if (field.TypeKey.Equals("general link"))
            {
                var link = (LinkField)field;

                value = new LinkFieldValueModel
                {
                    Description = link.Text,
                    TargetId = link.IsInternal ? link.TargetID.Guid : Guid.Empty,
                    TargetUrl = link.IsInternal ? "" : link.GetFriendlyUrl()
                };
            }
            else
            {
                value = field.Value;
            }

            return new FieldModel
            {
                Id = field.ID.Guid,
                Type = field.TypeKey,
                Key = field.Key,
                Value = value
            };
        }
    }
}