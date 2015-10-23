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
        private static readonly ID ControllerRenderingTemplateId = ID.Parse("{2A3E91A0-7987-44B5-AB34-35C2D9DE83B9}");
        private readonly JsonSerializer _serializer;

        public ItemSerializer()
        {
            _serializer = new JsonSerializer();
        }

        public void Serialize(Item item, Stream outputStream, string device, string mediaBaseUrl = null)
        {
            var @object = ConvertToReponseModel(item, device, mediaBaseUrl);

            using (TextWriter tx = new StreamWriter(outputStream))
            {
                using (var writer = new JsonTextWriter(tx))
                {
                    _serializer.Serialize(writer, @object);
                }
            }
        }

        private static ServerResponseModel ConvertToReponseModel(Item item, string deviceName, string mediaBaseUrl)
        {
            return new ServerResponseModel
            {
                Item = MapItem(item),
                Fields = MapFields(item, mediaBaseUrl),
                Presentation = MapPresentation(item, deviceName),
                Children = item.GetChildren().Select(child => new ServerResponseModel
                {
                    Item = MapItem(child),
                    Fields = MapFields(child, mediaBaseUrl)
                })
            };
        }

        private static ItemModel MapItem(Item item)
        {
            return new ItemModel
            {
                Id = item.ID.Guid,
                FullPath = item.Paths.FullPath.ToLowerInvariant(),
                Name = item.Name,
                ParentId = item.ParentID.Guid,
                TemplateId = item.TemplateID.Guid,
                HasVersion = item.Versions.Count > 0
            };
        }

        private static PresentationModel MapPresentation(Item item, string deviceName)
        {
            var device = item.Database.Resources.Devices["/sitecore/layout/devices/" + deviceName];

            if (device == null)
            {
                return null;
            }

            var layout = item.Visualization.GetLayout(device);

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

            var controllerRenderings = item.Visualization.GetRenderings(device, false)
                                           .Where(r => r.RenderingItem.InnerItem.TemplateID == ControllerRenderingTemplateId);

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
                        VaryByParm = rendering.Settings.Caching.VaryByParm,
                        VaryByQueryString = rendering.Settings.Caching.VaryByQueryString
                    }
                };
            });

            return presentation;
        }

        private static IEnumerable<FieldModel> MapFields(Item item, string mediaBaseUrl)
        {
            if (item.Versions.Count == 0)
            {
                return Enumerable.Empty<FieldModel>();
            }

            return item.Fields.Where(f => !f.Key.StartsWith("__")).Select(f => MapField(f, mediaBaseUrl)).Where(f => f != null);
        }

        private static FieldModel MapField(Field field, string mediaBaseUrl)
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