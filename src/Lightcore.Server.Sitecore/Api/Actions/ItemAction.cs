using System;
using System.Linq;
using System.Web;
using Lightcore.Server.Sitecore.Data;

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

        public override void Execute(HttpContextBase context, string query, Parameters parameters)
        {
            var item = parameters.Database.Items.GetItem(query, parameters.Language);

            if (item != null)
            {
                _serializer.SerializeItem(item, context.Response.OutputStream,
                    parameters.DeviceName,
                    parameters.ItemFields.ToArray(),
                    parameters.ChildFields.ToArray(),
                    parameters.Cdn);

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