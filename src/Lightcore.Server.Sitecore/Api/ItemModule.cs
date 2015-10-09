using System.Web;
using Lightcore.Server.Sitecore.Data;
using Sitecore.Configuration;
using Sitecore.Globalization;

namespace Lightcore.Server.Sitecore.Api
{
    public class ItemModule : IHttpModule
    {
        private readonly ItemToJsonConverter _itemConverter;

        public ItemModule()
        {
            _itemConverter = new ItemToJsonConverter();
        }

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
                    _itemConverter.Write(item, context.Response.OutputStream, device);

                    context.Response.ContentType = "application/json";
                }
                else
                {
                    context.Response.StatusCode = 404;
                }

                context.ApplicationInstance.CompleteRequest();
            };
        }

        public void Dispose()
        {
        }
    }
}