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
        private readonly string _apiPathPrefix = "/-/lightcore/item/";
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

                if (!path.StartsWith(_apiPathPrefix))
                {
                    return;
                }

                var cleanPath = path.Replace(_apiPathPrefix, "");
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

                var queryString = context.Request.QueryString;

                //// TODO: Validate parameters...
                var database = queryString["sc_database"] ?? "web";
                var device = queryString["sc_device"] ?? "default";
                var language = queryString["sc_lang"] ?? "en";
                var cdn = queryString["cdn"];
                var itemFields = queryString["itemfields"] != null ? queryString["itemfields"].Split(',') : new string[] {};
                var childFields = queryString["childfields"] != null ? queryString["childfields"].Split(',') : new string[] {};

                var item = Factory.GetDatabase(database).Items.GetItem(query, Language.Parse(language));

                if (item != null)
                {
                    _serializer.Serialize(item, context.Response.OutputStream, device, itemFields, childFields, cdn);

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