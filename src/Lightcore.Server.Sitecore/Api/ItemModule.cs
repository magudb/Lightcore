using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Lightcore.Server.Sitecore.Data;
using Sitecore.Configuration;
using Sitecore.Globalization;

namespace Lightcore.Server.Sitecore.Api
{
    public class ItemModule : IHttpModule
    {
        private const string ApiPathPrefix = "/-/lightcore/item/";
        private readonly Regex _guidRegex;
        private readonly ItemSerializer _serializer;

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
                var decodedPath = context.Server.UrlDecode(context.Request.Url.AbsolutePath);

                if (decodedPath == null)
                {
                    throw new HttpException((int)HttpStatusCode.BadRequest, "Bad request");
                }

                var path = decodedPath.ToLowerInvariant();

                if (!path.StartsWith(ApiPathPrefix))
                {
                    return;
                }

                var cleanPath = path.Replace(ApiPathPrefix, "");
                var isGuid = _guidRegex.Match(cleanPath);
                string query;

                if (isGuid.Success)
                {
                    query = "{" + isGuid.Value + "}";
                }
                else
                {
                    query = "/" + cleanPath;
                }

                //// TODO: Validate parameters...
                var database = context.Request.QueryString["sc_database"] ?? "web";
                var device = context.Request.QueryString["sc_device"] ?? "default";
                var language = context.Request.QueryString["sc_lang"] ?? "en";
                var cdn = context.Request.QueryString["cdn"];

                var item = Factory.GetDatabase(database).Items.GetItem(query, Language.Parse(language));

                if (item != null)
                {
                    _serializer.Serialize(item, context.Response.OutputStream, device, cdn);

                    context.Response.ContentType = "application/json";
                }
                else
                {
                    // TODO: Should we use response codes or should we set it on json response object?
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