using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lightcore.Server.Models;
using Newtonsoft.Json;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Lightcore.Server.Sitecore.Api
{
    public class ItemModule : IHttpModule
    {
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
                    var response = BuildResponseObject(item);
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

        private ServerResponseModel BuildResponseObject(Item item)
        {
            return new ServerResponseModel
            {
                Item = MapItem(item),
                Fields = MapFields(item),
                Presentation = MapPresentation(item),
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

        private PresentationModel MapPresentation(Item item)
        {
            // TODO: Looking to Sitecore.MVC ProcessXmlBasedLayoutDefinition
            // var x = new ProcessXmlBasedLayoutDefinition()
            // x.GetRenderings("xml from __Renderings field", null;


            // TODO: Look into why is this so slow? 

            //if (item.Visualization.Layout != null)
            //{
            //    var device = item.Database.Resources.Devices["/sitecore/layout/devices/default"];
            //    var renderings = item.Visualization.GetRenderings(device, false);

            //    return new LightcoreApiPresentation
            //    {
            //        Layout = new LightcoreApiLayout
            //        {
            //            Path = item.Visualization.GetLayout(device).FilePath
            //        },
            //        Renderings = renderings.Select(rendering => new LightcoreApiRendering
            //        {
            //            Placeholder = rendering.Placeholder,
            //            Controller = rendering.RenderingItem.InnerItem["Controller"]
            //        })
            //    };
            //}

            //return null;

            var presentation = new PresentationModel
            {
                Layout = new LayoutModel
                {
                    Path = item.Visualization.Layout.FilePath
                },
                Renderings = new List<RenderingModel>
                {
                    new RenderingModel
                    {
                        Placeholder = "content",
                        Controller = "Menu"
                    },
                    new RenderingModel
                    {
                        Placeholder = "content",
                        Controller = "Article"
                    },
                    new RenderingModel
                    {
                        Placeholder = "footer",
                        Controller = "Footer"
                    }
                }
            };

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