using Mall_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mall_Management.Controllers
{
    public class HomeController : Controller
    {
        mall_dbEntities db = new mall_dbEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Brands()
        {
            return View();
        }

        public ActionResult Events()
        {
            var events = db.Events.ToList(); 
            return View(events);
        }
        public ActionResult EventDetails()
        {
            var events = db.Events.ToList();
            return View(events);
        }

        public ActionResult Spaces()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult AmThuc()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}