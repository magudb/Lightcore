using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Core;
using Microsoft.AspNet.Routing;
using WebApp.Controllers;

namespace WebApp.Kernel
{
    public class LightcoreController : Controller
    {
        private readonly IControllerActivator _test;
        private readonly IActionInvokerFactory _test2;

        public LightcoreController(IControllerActivator test, IActionInvokerFactory test2)
        {
            _test = test;
            _test2 = test2;
        }

        public async Task<ActionResult> Render(string path)
        {
            //var data = new RouteData();
            //data.Values["controller"] = "Footer";
            //data.Values["action"] = "Index";

            //var ctx = new ActionContext(Context, data, new ActionDescriptor
            //{
            //    Name = "Index"
            //});
            //var x = (FooterController)_test.Create(ctx, typeof(FooterController));
            //var res = x.Index();
            //res.ExecuteResult(ctx);

            // ------- Works
            //var x = new FooterController();
            //var res = x.Index();
            //HttpContext ctx = new DefaultHttpContext();
            //ctx.ApplicationServices = Context.ApplicationServices;
            //ctx.RequestServices = Context.RequestServices;
            //var mem = new MemoryStream();
            //ctx.Response.Body = mem;
            //await res.ExecuteResultAsync(new ActionContext(ctx, new RouteData(), null));
            //mem.Position = 0;
            //StreamReader reader = new StreamReader(mem);
            //string text = reader.ReadToEnd();


            //---- Fails
           //var data = new RouteData();
           // data.Values["controller"] = "Footer";
           // data.Values["action"] = "Index";

           // var ctx = new ActionContext(Context, data, new ActionDescriptor
           // {
           //     Name = "Index"
           // });
           // var t = _test2.CreateInvoker(ctx);
           // await t.InvokeAsync();

            //---- Works I guess, if we copy response...
            //Controller controller = new FooterController();
            //controller.ActionContext = new ActionContext(Context, new RouteData(), null);
            //MethodInfo action = controller.GetType().GetMethod("Index");
            //await ControllerActionExecutor.ExecuteAsync(action, controller, new Dictionary<string, object>());

            var context = Context.LightcoreContext();

            // TODO: Full Item could be mapped to ItemModel to keep it simple?

            return View(context.Item.Layout, context.Item);
        }
    }
}