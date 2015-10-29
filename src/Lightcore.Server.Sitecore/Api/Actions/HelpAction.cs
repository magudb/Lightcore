using System.Configuration;
using System.Web;
using Sitecore.WordOCX.Extensions;

namespace Lightcore.Server.Sitecore.Api.Actions
{
    public class HelpAction : ModuleAction
    {
        private readonly bool _cmEnabled;

        public HelpAction()
        {
            bool.TryParse(ConfigurationManager.AppSettings["Lightcore.Server.Sitecore.CmEnabled"], out _cmEnabled);
        }

        public override string HandlesPath
        {
            get { return "/"; }
        }

        public override bool CanHandle(string path)
        {
            if (_cmEnabled)
            {
                return path.Equals(HandlesPath);
            }

            return true;
        }

        public override void Execute(HttpContext context, string query, string database, string device, string language)
        {
            context.Response.WriteLine("Help...");
        }
    }
}