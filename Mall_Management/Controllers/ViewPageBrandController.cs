using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mall_Management.Models;

namespace Mall_Management.Controllers
{
    public class ViewPageBrandController : Controller
    {
        private readonly mall_dbEntities db = new mall_dbEntities();

        // GET: Brand
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AmThuc()
        {
            ViewBag.listAmThuc = db.get("exec TIMKIEMBRANDSTHEOCATEID '1'");
            return View();
        }
        public ActionResult ThoiTrang()
        {
            ViewBag.listThoiTrang = db.get("exec TIMKIEMBRANDSTHEOCATEID '2'");
            return View();
        }
        public ActionResult HoctapVaGiaiTri()
        {
            ViewBag.listHocTap = db.get("exec TIMKIEMBRANDSTHEOCATEID '3'");
            return View();
        }
        public ActionResult TangB()
        {
            ViewBag.listTangB = db.get("exec TIMBRANDSTHEOFLOOR 'B'");
            return View();
        }
        public ActionResult Tang1()
        {
            ViewBag.listTang1 = db.get("exec TIMBRANDSTHEOFLOOR '1'");
            return View();
        }
        public ActionResult Tang2()
        {
            ViewBag.listTang2 = db.get("exec TIMBRANDSTHEOFLOOR '2'");
            return View();
        }
        public ActionResult Tang3()
        {
            ViewBag.listTang3 = db.get("exec TIMBRANDSTHEOFLOOR '3'");
            return View();
        }
        public ActionResult Tang4()
        {
            ViewBag.listTang4 = db.get("exec TIMBRANDSTHEOFLOOR '4'");
            return View();
        }
        public ActionResult Tang5()
        {
            ViewBag.listTang5 = db.get("exec TIMBRANDSTHEOFLOOR '5'");
            return View();
        }
        public ActionResult Tang6()
        {
            ViewBag.listTang6 = db.get("exec TIMBRANDSTHEOFLOOR '6'");
            return View();
        }
    }
}