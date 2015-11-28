using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lightcore.Server.Sitecore.Api.Actions;
using Lightcore.Server.Sitecore.Data;

namespace Lightcore.Server.Sitecore.Api
{
    public class ItemModule : IHttpModule
    {
        private readonly IEnumerable<ModuleAction> _actions;

        public ItemModule()
        {
            var serializer = new ItemSerializer();

            _actions = new ModuleAction[]
            {
                new ItemAction(serializer),
                new VersionsAction(serializer),
                new HelpAction()
            };
        }

        public void Init(HttpApplication app)
        {
            app.BeginRequest += (sender, args) =>
            {
                var context = new HttpContextWrapper(((HttpApplication)sender).Context);

                // ReSharper disable PossibleNullReferenceException
                var decodedPath = context.Server.UrlDecode(context.Request.Url.AbsolutePath);
                // ReSharper restore PossibleNullReferenceException

                if (decodedPath == null)
                {
                    throw new HttpException((int)HttpStatusCode.BadRequest, "Bad request");
                }

                var path = decodedPath.ToLowerInvariant();

                foreach (var action in _actions.Where(action => action.CanHandle(path)))
                {
                    action.DoExecute(context, path);

                    context.ApplicationInstance.CompleteRequest();

                    break;
                }
            };
        }

        public void Dispose()
        {
        }
    }
}