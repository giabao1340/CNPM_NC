﻿using Mall_Management.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall_Management.Controllers
{
    public class BrandController : Controller
    {
        private readonly mall_dbEntities _db = new mall_dbEntities();

        // GET: Areas/Brands
        public ActionResult Index(string search, int? size, int? page)
        {
            var pageSize = (size ?? 15);
            var pageNumber = (page ?? 1);
            ViewBag.search = search;
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

        [HttpPost]
        public JsonResult Create(Brand brand)
        {
            string result = "false";
            try
            {
                // Kiểm tra nếu thương hiệu đã tồn tại
                Brand checkExist = _db.Brands.SingleOrDefault(m => m.BrandName == brand.BrandName);
                if (checkExist != null)
                {
                    result = "exist";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                // Thêm thương hiệu mới vào cơ sở dữ liệu
                _db.Brands.Add(brand);
                _db.SaveChanges();
                result = "success";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult Edit(int id, Brand brand)
        {
            string result = "error";
            try
            {
                // Tìm thương hiệu cần sửa
                Brand brandToUpdate = _db.Brands.FirstOrDefault(m => m.BrandID == id);

                // Nếu không tìm thấy thương hiệu
                if (brandToUpdate == null)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                // Kiểm tra nếu tên thương hiệu đã tồn tại cho một thương hiệu khác
                var checkExist = _db.Brands.FirstOrDefault(m => m.BrandName == brand.BrandName && m.BrandID != id);
                if (checkExist != null)
                {
                    result = "exist";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                // Cập nhật thông tin thương hiệu
                brandToUpdate.BrandName = brand.BrandName;
                brandToUpdate.Image = brand.Image;
                brandToUpdate.Description = brand.Description;
                brandToUpdate.Url = brand.Url;

                _db.Entry(brandToUpdate).State = EntityState.Modified;
                _db.SaveChanges();
                result = "success";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult Delete(int id)
        {
            string result = "error";
            try
            {
                // Tìm thương hiệu cần xóa
                Brand brand = _db.Brands.FirstOrDefault(m => m.BrandID == id);

                // Nếu không tìm thấy thương hiệu
                if (brand == null)
                {
                    return Json(new { success = false, message = "Brand not found" }, JsonRequestBehavior.AllowGet);
                }

                // Xóa thương hiệu
                _db.Brands.Remove(brand);
                _db.SaveChanges();
                result = "success";
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return Json(new { success = false, message = "Error occurred while deleting the brand" }, JsonRequestBehavior.AllowGet);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }

    }
}