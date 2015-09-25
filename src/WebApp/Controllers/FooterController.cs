using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace WebApp.Controllers
{
    public class FooterController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
