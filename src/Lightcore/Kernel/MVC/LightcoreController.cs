using Lightcore.Kernel.Http;
using Microsoft.AspNet.Mvc;

namespace Lightcore.Kernel.MVC
{
    public class LightcoreController : Controller
    {
        public ActionResult Render(string path)
        {
            // TODO: What to do about favico.ico?

            if (!string.IsNullOrEmpty(path) && path.Contains("."))
            {
                return null;
            }

            var context = Context.LightcoreContext();

            // TODO: Full Item could be mapped to ItemModel to keep it simple?

            return View(context.Item.Layout, context.Item);
        }
    }
}