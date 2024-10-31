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
using System.Data.Entity.Validation;
using System.Drawing.Drawing2D;
using System.IO;

namespace Mall_Management.Controllers
{
    public class EventsController : Controller
    {
        private mall_dbEntities db = new mall_dbEntities();

        // GET: Events
        public async Task<ActionResult> Index()
        {
            var events = db.Events.Include(m => m.Brand);
            return View(await events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName");
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventID,EventName,Description,Image,StartDate,EndDate,BrandID,Register")] Event events, HttpPostedFileBase image)
        {
            // Kiểm tra nếu tên thương hiệu đã tồn tại
            var existingEvent = db.Events.FirstOrDefault(b => b.EventName == events.EventName);
            if (existingEvent != null)
            {
                ModelState.AddModelError("EventName", "Thương hiệu đã tồn tại.");
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
                    events.Image = "/images/" + fileName;
                }

                // Lưu đối tượng Brand vào cơ sở dữ liệu
                db.Events.Add(events);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "BrandID", "BrandName", events.BrandID);
            return View(events);
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create2([Bind(Include = "EventID,EventName,Description,Image,StartDate,EndDate,BrandID,Register")] Event @event)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Events.Add(@event);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException e)
                {
                    // Ghi chi tiết lỗi vào ModelState để hiển thị trong View
                    foreach (var validationErrors in e.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }

            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", @event.BrandID);
            return View(@event);
        }


        // GET: Events/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return HttpNotFound();
            }

            // Tạo SelectList cho BrandID
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", @event.BrandID);
            return View(@event);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EventID,EventName,Description,Image,StartDate,EndDate,BrandID,Register")] Event events, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu tên sự kiện đã tồn tại, bỏ qua sự kiện hiện tại
                var existingEvent = db.Events.FirstOrDefault(b => b.EventName == events.EventName && b.EventID != events.EventID);
                if (existingEvent != null)
                {
                    ModelState.AddModelError("EventName", "Sự kiện đã tồn tại.");
                    ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", events.BrandID);
                    return View(events);
                }

                // Xử lý ảnh tải lên
                if (image != null && image.ContentLength > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/"), fileName);
                    image.SaveAs(path);
                    events.Image = "/images/" + fileName;
                }
                else
                {
                    events.Image = db.Events.AsNoTracking().Where(b => b.EventID == events.EventID).Select(b => b.Image).FirstOrDefault();
                }

                db.Entry(events).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // Đảm bảo SelectList cho BrandID khi có lỗi
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", events.BrandID);
            return View(events);
        }


        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var events = db.Events.Find(id);
                if (events == null)
                {
                    return Json(new { success = false, message = "Thương hiệu không tồn tại." });
                }

                db.Events.Remove(events);
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
