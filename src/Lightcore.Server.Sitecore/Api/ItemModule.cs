using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lightcore.Server.Models;
using Newtonsoft.Json;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Resources.Media;

namespace Lightcore.Server.Sitecore.Api
{
    public class ItemModule : IHttpModule
    {
        private readonly ID _controllerRenderingTemplateId = ID.Parse("{2A3E91A0-7987-44B5-AB34-35C2D9DE83B9}");

        public void Init(HttpApplication app)
        {
            app.BeginRequest += (sender, args) =>
            {
                var context = ((HttpApplication)sender).Context;

                // ReSharper disable once PossibleNullReferenceException
                var path = context.Server.UrlDecode(context.Request.Url.AbsolutePath).ToLowerInvariant();

                if (!path.StartsWith("/-/lightcore/item/"))
                {
                    return;
                }

                //// TODO: Should we use response codes or should we set it on json response object?

                var pathOrId = path.Replace("/-/lightcore/item/", "/");
                var database = context.Request.QueryString["sc_database"] ?? "web";
                var device = context.Request.QueryString["sc_device"] ?? "default";
                var language = context.Request.QueryString["sc_lang"] ?? "en";
                var item = Factory.GetDatabase(database).Items.GetItem(pathOrId, Language.Parse(language));

                if (item != null)
                {
                    // TODO: Set from querystring or configuration ...
                    var response = BuildResponseObject(item, device);
                    var json = JsonConvert.SerializeObject(response);

                    context.Response.ContentType = "application/json";
                    context.Response.Write(json);
                }
                else
                {
                    context.Response.StatusCode = 404;
                }

                context.Items["SitecoreOn"] = false; // TODO: Check if needed?
                context.ApplicationInstance.CompleteRequest();
            };
        }

        public void Dispose()
        {
        }

        private ServerResponseModel BuildResponseObject(Item item, string deviceName)
        {
            return new ServerResponseModel
            {
                Item = MapItem(item),
                Fields = MapFields(item),
                Presentation = MapPresentation(item, deviceName),
                Children = item.GetChildren().Select(child => new ServerResponseModel
                {
                    Item = MapItem(child),
                    Fields = MapFields(child),
                    Presentation = null,
                    Children = Enumerable.Empty<ServerResponseModel>()
                })
            };
        }

        private ItemModel MapItem(Item item)
        {
            return new ItemModel
            {
                Id = item.ID.Guid,
                FullPath = item.Paths.FullPath.ToLowerInvariant(),
                Name = item.Name,
                ParentId = item.ParentID.Guid,
                TemplateId = item.TemplateID.Guid,
                TemplateName = item.TemplateName
            };
        }

        private PresentationModel MapPresentation(Item item, string deviceName)
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

            var controllerRenderings = item.Visualization.GetRenderings(device, false).Where(r => r.RenderingItem.InnerItem.TemplateID == _controllerRenderingTemplateId);

            presentation.Renderings = controllerRenderings.Select(rendering =>
            {
                var action = rendering.RenderingItem.InnerItem["Action"];

                return new RenderingModel
                {
                    Placeholder = rendering.Placeholder,
                    Controller = rendering.RenderingItem.InnerItem["Controller"],
                    Action = !string.IsNullOrEmpty(action) ? action : "Index"
                };
            });

            return presentation;
        }

        private IEnumerable<FieldModel> MapFields(BaseItem item)
        {
            foreach (var field in item.Fields.Where(f => !f.Key.StartsWith("__")))
            {
                yield return MapField(field);
            }
        }

        private FieldModel MapField(Field field)
        {
            string value;

            if (field.TypeKey.Equals("image"))
            {
                var media = (ImageField)field;

                value = MediaManager.GetMediaUrl(media.MediaItem, new MediaUrlOptions
                {
                    AlwaysIncludeServerUrl = true,
                    IncludeExtension = true,
                    LowercaseUrls = true,
                    UseItemPath = true
                });
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