using Mall_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall_Management.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ThuongHieu()
        {
            return View();
        }
        public ActionResult DanhMuc()
        {
            return View();
        }
        public ActionResult UuDai()
        {
            return View();
        }
        public ActionResult SuKien()
        {
            return View();
        }
        public ActionResult MatBang()
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