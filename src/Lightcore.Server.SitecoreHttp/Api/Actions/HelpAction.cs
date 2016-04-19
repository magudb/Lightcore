using System.Configuration;
using System.Web;

namespace Lightcore.Server.SitecoreHttp.Api.Actions
{
    public class HelpAction : ModuleAction
    {
        private readonly bool _cmEnabled;

        public HelpAction()
        {
            bool.TryParse(ConfigurationManager.AppSettings["Lightcore.Server.SitecoreHttp.CmEnabled"], out _cmEnabled);
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

        public override void Execute(HttpContextBase context, string query, Parameters parameters)
        {
            context.Response.Write("Help...");
        }
    }
}