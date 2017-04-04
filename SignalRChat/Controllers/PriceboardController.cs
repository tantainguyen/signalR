using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRChat.Controllers
{
    public class PriceboardController : Controller
    {
        // GET: Priceboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult API()
        {
            return View("~/Views/Priceboard/IndexAPI.cshtml");
        }
        public ActionResult HNX() {
            return View("~/Views/Priceboard/HNX.cshtml");
        }

        public ActionResult HSX()
        {
            return View("~/Views/Priceboard/HSX.cshtml");
        }
    }
}