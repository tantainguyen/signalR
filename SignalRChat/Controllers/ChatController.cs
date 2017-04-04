using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRChat.Controllers
{
    public class ChatController : Controller
    {
        // GET: User
        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                ViewData["UserName"] = id;
                return View("~/Views/Chat/User.cshtml");
            }
        }
    }
}