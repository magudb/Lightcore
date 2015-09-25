using Microsoft.AspNet.Mvc;

namespace WebApp.Controllers
{
    public class ArticleController : Controller
    {
        public ActionResult Index()
        {
            return View("/Views/Article/Index.cshtml");
        }
    }
}