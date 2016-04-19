using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using Sitecore.Data;

namespace Lightcore.Server.SitecoreHttp.Api.Actions
{
    public abstract class ModuleAction
    {
        private readonly Regex _guidRegex;
        private readonly List<Database> _databases;

        protected ModuleAction()
        {
            _guidRegex = new Regex(@"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _databases = global::Sitecore.Configuration.Factory.GetDatabases();
        }

        public abstract string HandlesPath { get; }

        public abstract bool CanHandle(string path);

        public abstract void Execute(HttpContextBase context, string query, Parameters parameters);

        internal void DoExecute(HttpContextBase context, string decodedPath)
        {
            var parameters = new Parameters(context.Request, _databases);

            if (!parameters.IsValid())
            {
                context.Response.StatusCode = 400;

                return;
            }
            
            var relativePath = decodedPath.Replace(HandlesPath, "/");
            var isGuidMatch = _guidRegex.Match(relativePath);
            var query = isGuidMatch.Success ? string.Concat("{", isGuidMatch.Value, "}") : string.Concat("/", relativePath);

            Execute(context, query, parameters);
        }
    }
}