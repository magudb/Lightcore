using Microsoft.AspNet.Mvc;

namespace Lightcore.Kernel.MVC
{
    public class LightcoreController : Controller
    {
        public ActionResult Render(string path)
        {
            var context = Context.LightcoreContext();

            return View(context.Item.Visualization.Layout.Path, context.Item);
        }
    }
}