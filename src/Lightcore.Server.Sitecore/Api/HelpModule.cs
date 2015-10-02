using System.Text;
using System.Web;

namespace Lightcore.Server.Sitecore.Api
{
    public class HelpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, args) =>
            {
                var app = (HttpApplication)sender;
                var path = HttpContext.Current.Request.Url.AbsolutePath.ToLowerInvariant();

                if (!path.Equals("/"))
                {
                    return;
                }

                const string itemTestUrl = "/-/lightcore/item/sitecore/content/home?sc_database=web&sc_lang=en&sc_device=default";
                var content = new StringBuilder();

                content.Append("<html>");
                content.Append("<head>");
                content.AppendFormat("<title>Lightcore Server</title>");
                content.Append("</head>");
                content.Append("<body>");
                content.Append("<h1>Lightcore Server</h1>");
                content.AppendFormat("<p>Item: <a href=\"{0}\">{0}</a></p>", itemTestUrl);
                content.Append("</body>");
                content.Append("</html>");

                app.Response.Write(content);
                app.Response.ContentType = "text/html";
                app.CompleteRequest();
            };
        }

        public void Dispose()
        {
        }
    }
}