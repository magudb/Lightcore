using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using ItemSerializer = Lightcore.Server.SitecoreHttp.Data.ItemSerializer;

namespace Lightcore.Server.SitecoreHttp.Api.Actions
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

        public override void Execute(HttpContextBase context, string query, Parameters parameters)
        {
            var item = parameters.Database.Items.GetItem(query, parameters.Language);

            if (item != null)
            {
                var items = new List<Item>
                {
                    item
                };

                foreach (var language in item.Languages.Where(x => !x.Equals(parameters.Language)))
                {
                    var languageItem = item.Database.Items.GetItem(item.ID, language);

                    items.Add(languageItem);
                }

                _serializer.SerializeVersions(items, context.Response.OutputStream, parameters.ItemFields.ToArray(), parameters.Cdn);

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