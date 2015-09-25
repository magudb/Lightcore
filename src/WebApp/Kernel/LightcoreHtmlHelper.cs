using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Routing;

namespace WebApp.Kernel
{
    public class LightcoreHtmlHelper
    {
        private readonly IHtmlHelper _htmlHelper;

        public LightcoreHtmlHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public HtmlString Placeholder(string name)
        {
            // TODO: Run pipeline...

            var builder = new StringBuilder();
            var context = _htmlHelper.LightcoreContext();

            builder.Append("<div style=\"border: 1px solid red; padding:5px; margin:5px;\">");
            builder.Append($"<p>PLACEHOLDER:{name} ({context.Item.Key}, {context.Item.Id})</p>");

            foreach (var key in context.Item.Renderings.Keys)
            {
                if (!key.EndsWith(name))
                {
                    continue;
                }

                var httpContext = _htmlHelper.ViewContext.HttpContext;
                var rendering = context.Item.Renderings[key];
                var controllerType = Type.GetType($"WebApp.Controllers.{rendering}Controller");
                string text;

                builder.Append($"<p>Adding rendering '{controllerType.FullName}'...</p>");

                var controllerInstance = (Controller)Activator.CreateInstance(controllerType);
                MethodInfo actionMethod = controllerType.GetMethod("Index");

                var actionResult = (ActionResult)actionMethod.Invoke(controllerInstance, new object[0]);

                using (var responseStream = new MemoryStream())
                {
                    var oldBody = httpContext.Response.Body;

                    // TODO: This is some nasty shit.. and only works on first request :)
                    httpContext.Response.Body = responseStream;

                    actionResult.ExecuteResultAsync(new ActionContext(httpContext, new RouteData(), null));

                    responseStream.Position = 0;

                    using (var reader = new StreamReader(responseStream))
                    {
                        text = reader.ReadToEnd();
                    }

                    httpContext.Response.Body = oldBody;
                }

                builder.Append(text);
            }

            builder.Append("</div>");

            return new HtmlString(builder.ToString());
        }
    }
}