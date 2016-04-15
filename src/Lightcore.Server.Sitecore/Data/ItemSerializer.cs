using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lightcore.Server.Models;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;

namespace Lightcore.Server.Sitecore.Data
{
    public class ItemSerializer
    {
        private readonly ID _controllerRenderingTemplateId = ID.Parse("{2A3E91A0-7987-44B5-AB34-35C2D9DE83B9}");
        private readonly JsonSerializer _serializer;

        public ItemSerializer()
        {
            _serializer = new JsonSerializer();
        }

        public void SerializeItem(Item item, Stream outputStream, string device,
                                  string[] itemFields = null,
                                  string[] childFields = null,
                                  string mediaBaseUrl = null)
        {
            var @object = CreateResponseModelForItem(item, device, itemFields, childFields, mediaBaseUrl);

            using (TextWriter textWriter = new StreamWriter(outputStream))
            {
                using (var jsonWriter = new JsonTextWriter(textWriter))
                {
                    _serializer.Serialize(jsonWriter, @object);
                }
            }
        }

        public void SerializeVersions(IEnumerable<Item> items, Stream outputStream,
                                      string[] itemFields = null,
                                      string mediaBaseUrl = null)
        {
            var @object = CreateResponseModelForVersions(items, itemFields, mediaBaseUrl);

            using (TextWriter textWriter = new StreamWriter(outputStream))
            {
                using (var jsonWriter = new JsonTextWriter(textWriter))
                {
                    _serializer.Serialize(jsonWriter, @object);
                }
            }
        }

        private ServerResponseModel CreateResponseModelForItem(Item item, string device,
                                                               string[] itemFields,
                                                               string[] childFields,
                                                               string mediaBaseUrl)
        {
            return new ServerResponseModel
            {
                Items = new[]
                {
                    new ItemModel
                    {
                        Properties = MapItem(item),
                        Fields = MapFields(item, itemFields, mediaBaseUrl),
                        Presentation = MapPresentation(item, device),
                        Children = item.GetChildren().Select(child => new ItemModel
                        {
                            Properties = MapItem(child),
                            Fields = MapFields(child, childFields, mediaBaseUrl)
                        })
                    }
                }
            };
        }

        private ServerResponseModel CreateResponseModelForVersions(IEnumerable<Item> items, string[] itemFields, string mediaBaseUrl)
        {
            var itemModels = new List<ItemModel>();

            foreach (var item in items)
            {
                itemModels.Add(new ItemModel
                {
                    Properties = MapItem(item),
                    Fields = MapFields(item, itemFields, mediaBaseUrl),
                    Presentation = null,
                    Children = Enumerable.Empty<ItemModel>()
                });
            }

            return new ServerResponseModel
            {
                Items = itemModels.ToArray()
            };
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

        private PresentationModel MapPresentation(Item item, string device)
        {
            var deviceItem = item.Database.Resources.Devices["/sitecore/layout/devices/" + device];

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

        private IEnumerable<FieldModel> MapFields(Item item, string[] specificFields, string mediaBaseUrl)
        {
            if (item.Versions.Count == 0)
            {
                return Enumerable.Empty<FieldModel>();
            }

            var fields = new List<FieldModel>();

            if (!specificFields.Any())
            {
                return item.Fields.Where(f => !f.Key.StartsWith("__")).Select(f => MapField(f, mediaBaseUrl)).Where(f => f != null);
            }

            foreach (var key in specificFields)
            {
                var f = item.Fields[key.Trim()];

                if (f != null)
                {
                    fields.Add(MapField(f, mediaBaseUrl));
                }
            }

            return fields.Where(f => f != null);
        }

        private FieldModel MapField(Field field, string mediaBaseUrl)
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
                        MediaLinkServerUrl = mediaBaseUrl,
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