using Microsoft.AspNet.Mvc;

namespace DemoWebsite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("Home.Index");
        }
    }
}