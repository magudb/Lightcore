using System;
using System.Web;
using Lightcore.Server.Sitecore.Data;
using Sitecore.Configuration;
using Sitecore.Globalization;

namespace Lightcore.Server.Sitecore.Api.Actions
{
    public class ItemAction : ModuleAction
    {
        private readonly ItemSerializer _serializer;

        public ItemAction(ItemSerializer serializer)
        {
            _serializer = serializer;
        }

        public override string HandlesPath
        {
            get { return "/-/lightcore/item/"; }
        }

        public override bool CanHandle(string path)
        {
            return path.StartsWith(HandlesPath, StringComparison.OrdinalIgnoreCase);
        }

        public override void Execute(HttpContext context, string query, string database, string device, string language)
        {
            var queryString = context.Request.QueryString;
            var cdn = queryString["cdn"];
            var itemFields = queryString["itemfields"] != null ? queryString["itemfields"].Split(',') : new string[] {};
            var childFields = queryString["childfields"] != null ? queryString["childfields"].Split(',') : new string[] {};
            var item = Factory.GetDatabase(database).Items.GetItem(query, Language.Parse(language));

            if (item != null)
            {
                _serializer.SerializeItem(item, context.Response.OutputStream, device, itemFields, childFields, cdn);

                context.Response.ContentType = "application/json";
            }
            else
            {
                // TODO: Should we use response codes or should we set it on json response object?
                context.Response.StatusCode = 404;
            }
        }
    }
}