using Mall_Management.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall_Management.Controllers
{
    public class BrandController : Controller
    {
        private readonly mall_dbEntities _db = new mall_dbEntities();


        // GET: Brand
        public ActionResult Index(string search, int? size, int? page)
        {
            var pageSize = (size ?? 15);
            var pageNumber = (page ?? 1);
            var list = from a in _db.Brands
                       orderby a.BrandID descending
                       select a;
            if (!string.IsNullOrEmpty(search))
            {
                list = from a in _db.Brands
                       where a.BrandName.Contains(search)
                       orderby a.BrandID descending
                       select a;
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult AmThuc()
        {
            return View();
        }
    }
}