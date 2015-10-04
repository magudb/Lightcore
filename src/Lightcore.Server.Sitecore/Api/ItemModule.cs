using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lightcore.Server.Models;
using Newtonsoft.Json;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Lightcore.Server.Sitecore.Api
{
    public class ItemModule : IHttpModule
    {
        private readonly ID _controllerRenderingTemplateId = ID.Parse("{2A3E91A0-7987-44B5-AB34-35C2D9DE83B9}");

        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, args) =>
            {
                var path = HttpContext.Current.Request.Url.AbsolutePath.ToLowerInvariant();

                if (!path.StartsWith("/-/lightcore/item/"))
                {
                    return;
                }

                var pathOrId = path.Replace("/-/lightcore/item/", "/");
                var database = HttpContext.Current.Request.QueryString["sc_database"] ?? "web";
                var device = HttpContext.Current.Request.QueryString["sc_device"] ?? "default";
                var language = HttpContext.Current.Request.QueryString["sc_lang"] ?? "en";

                var item = Factory.GetDatabase(database).Items.GetItem(pathOrId, Language.Parse(language));

                if (item != null)
                {
                    var response = BuildResponseObject(item, device);
                    var json = JsonConvert.SerializeObject(response);

                    HttpContext.Current.Response.ContentType = "application/json";
                    HttpContext.Current.Response.Write(json);
                }
                else
                {
                    HttpContext.Current.Response.StatusCode = 404;
                }

                HttpContext.Current.Items["SitecoreOn"] = false; // TODO: Needed?
                HttpContext.Current.ApplicationInstance.CompleteRequest();
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
            var fields = new List<FieldModel>();

            foreach (var field in item.Fields.Where(f => !f.Key.StartsWith("__")))
            {
                fields.Add(new FieldModel
                {
                    Id = field.ID.Guid,
                    Type = field.TypeKey,
                    Key = field.Key,
                    Value = field.Value
                });
            }

            return fields;
        }
    }
}