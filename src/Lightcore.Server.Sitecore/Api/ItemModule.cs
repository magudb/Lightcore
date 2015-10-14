using System.Text.RegularExpressions;
using System.Web;
using Lightcore.Server.Sitecore.Data;
using Sitecore.Configuration;
using Sitecore.Globalization;

namespace Lightcore.Server.Sitecore.Api
{
    public class ItemModule : IHttpModule
    {
        private readonly ItemSerializer _serializer;
        private readonly Regex _guidRegex;

        public ItemModule()
        {
            _serializer = new ItemSerializer();
            _guidRegex = new Regex(@"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
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
                
                var cleanPath = path.Replace("/-/lightcore/item/", "");
                var isGuid =_guidRegex.Match(cleanPath);
                string query;

                if (isGuid.Success)
                {
                    query = "{" + isGuid.Value + "}";
                }
                else
                {
                    query = "/" + cleanPath;
                }
                
                var database = context.Request.QueryString["sc_database"] ?? "web";
                var device = context.Request.QueryString["sc_device"] ?? "default";
                var language = context.Request.QueryString["sc_lang"] ?? "en";
                var item = Factory.GetDatabase(database).Items.GetItem(query, Language.Parse(language));

                if (item != null)
                {
                    _serializer.Serialize(item, context.Response.OutputStream, device);

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