using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mall_Management.Models;
using System.IO;

namespace Mall_Management.Controllers
{
    public class BrandsController : Controller
    {
        private mall_dbEntities db = new mall_dbEntities();

        // GET: Brands
        public async Task<ActionResult> Index()
        {
            var brands = db.Brands.Include(b => b.Category);
            return View(await brands.ToListAsync());
        }

        // GET: Brands/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = await db.Brands.FindAsync(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // GET: Brands/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Create([Bind(Include = "BrandID,BrandName,Url,Description,Floor,CategoryID")] Brand brand, HttpPostedFileBase image)
{
    // Kiểm tra nếu tên thương hiệu đã tồn tại
    var existingBrand = db.Brands.FirstOrDefault(b => b.BrandName == brand.BrandName);
    if (existingBrand != null)
    {
        ModelState.AddModelError("BrandName", "Thương hiệu đã tồn tại.");
    }

    if (ModelState.IsValid)
    {
        // Kiểm tra xem file image có được chọn không
        if (image != null && image.ContentLength > 0)
        {
            // Tạo tên file duy nhất và lưu vào thư mục
            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            var path = Path.Combine(Server.MapPath("~/images/"), fileName);
            image.SaveAs(path);

            // Lưu đường dẫn ảnh vào thuộc tính Image của đối tượng Brand
            brand.Image = "/images/" + fileName;
        }

        // Lưu đối tượng Brand vào cơ sở dữ liệu
        db.Brands.Add(brand);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", brand.CategoryID);
    return View(brand);
}


        // GET: Brands/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = await db.Brands.FindAsync(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", brand.CategoryID);
            return View(brand);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BrandID,BrandName,Image,Url,Description,Floor,CategoryID")] Brand brand, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu tên thương hiệu đã tồn tại, nhưng bỏ qua thương hiệu hiện tại
                var existingBrand = db.Brands.FirstOrDefault(b => b.BrandName == brand.BrandName && b.BrandID != brand.BrandID);
                if (existingBrand != null)
                {
                    ModelState.AddModelError("BrandName", "Thương hiệu đã tồn tại.");
                    ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", brand.CategoryID);
                    return View(brand);
                }

                // Kiểm tra xem file image có được chọn không
                if (image != null && image.ContentLength > 0)
                {
                    // Tạo tên file duy nhất và lưu vào thư mục
                    var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/"), fileName);
                    image.SaveAs(path);

                    // Lưu đường dẫn ảnh vào thuộc tính Image của đối tượng Brand
                    brand.Image = "/images/" + fileName;
                }
                // Nếu không tải ảnh mới, giữ nguyên ảnh cũ
                else
                {
                    brand.Image = db.Brands.AsNoTracking().Where(b => b.BrandID == brand.BrandID).Select(b => b.Image).FirstOrDefault();
                }

                // Đánh dấu đối tượng Brand là đã được sửa đổi
                db.Entry(brand).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", brand.CategoryID);
            return View(brand);
        }


        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var brand = db.Brands.Find(id);
                if (brand == null)
                {
                    return Json(new { success = false, message = "Thương hiệu không tồn tại." });
                }

                db.Brands.Remove(brand);
                db.SaveChanges();

                return Json(new { success = true, message = "Xóa thành công." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
