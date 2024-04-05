using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INTERNS_HUB.Models;

namespace INTERNS_HUB.Controllers
{
    public class InternsHubController : Controller
    {
        // GET: InternsHub
        public ActionResult InternsHub_MainView()
        {
            return View();
        }

        public ActionResult About_PageLoad()
        {
            return View();
        }

        public ActionResult Work_PageLoad()
        {
            return View();
        }

        public ActionResult Category_PageLoad()
        {
            return View();
        }
    }
}