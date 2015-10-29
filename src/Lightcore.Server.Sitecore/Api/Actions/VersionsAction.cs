using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using ItemSerializer = Lightcore.Server.Sitecore.Data.ItemSerializer;

namespace Lightcore.Server.Sitecore.Api.Actions
{
    public class VersionsAction : ModuleAction
    {
        private readonly ItemSerializer _serializer;

        public VersionsAction(ItemSerializer serializer)
        {
            _serializer = serializer;
        }

        public override string HandlesPath
        {
            get { return "/-/lightcore/versions/"; }
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
            var item = Factory.GetDatabase(database).Items.GetItem(query, Language.Parse(language));

            if (item != null)
            {
                var items = new List<Item>
                {
                    item
                };

                foreach (var languageVersion in item.Languages.Where(lang => !lang.Name.Equals(language, StringComparison.OrdinalIgnoreCase)))
                {
                    var languageItem = item.Database.Items.GetItem(item.ID, languageVersion);

                    items.Add(languageItem);
                }

                _serializer.SerializeVersions(items, context.Response.OutputStream, itemFields, cdn);

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