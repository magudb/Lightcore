using System.Text.RegularExpressions;
using System.Web;

namespace Lightcore.Server.Sitecore.Api.Actions
{
    public abstract class ModuleAction
    {
        private readonly Regex _guidRegex;

        protected ModuleAction()
        {
            _guidRegex = new Regex(@"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public abstract string HandlesPath { get; }

        public abstract bool CanHandle(string path);

        public abstract void Execute(HttpContext context, string query, string database, string device, string language);

        internal void DoExecute(HttpContext context, string decodedPath)
        {
            var relativePath = decodedPath.Replace(HandlesPath, "/");
            var isGuid = _guidRegex.Match(relativePath);
            string query;

            if (isGuid.Success)
            {
                query = "{" + isGuid.Value + "}";
            }
            else
            {
                query = "/" + relativePath;
            }

            //// TODO: Validate parameters...
            var database = context.Request.QueryString["sc_database"] ?? "web";
            var device = context.Request.QueryString["sc_device"] ?? "default";
            var language = context.Request.QueryString["sc_lang"] ?? "en";

            Execute(context, query, database, device, language);
        }
    }
}