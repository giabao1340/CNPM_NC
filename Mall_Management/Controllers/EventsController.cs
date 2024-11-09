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
                // Fetch existing event data to retain unmodified values
                var existingEvent = await db.Events.AsNoTracking().FirstOrDefaultAsync(b => b.EventID == events.EventID);
                if (existingEvent == null)
                {
                    return HttpNotFound();
                }

                // Check if EventName is unique, excluding the current event
                var duplicateEvent = db.Events.FirstOrDefault(b => b.EventName == events.EventName && b.EventID != events.EventID);
                if (duplicateEvent != null)
                {
                    ModelState.AddModelError("EventName", "Sự kiện đã tồn tại.");
                    ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", events.BrandID);
                    return View(events);
                }

                // Retain original StartDate and EndDate if they aren't updated
                events.StartDate = events.StartDate == default ? existingEvent.StartDate : events.StartDate;
                events.EndDate = events.EndDate == default ? existingEvent.EndDate : events.EndDate;

                // Handle image upload
                if (image != null && image.ContentLength > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/"), fileName);
                    image.SaveAs(path);
                    events.Image = "/images/" + fileName;
                }
                else
                {
                    events.Image = existingEvent.Image; // Keep existing image if no new image is uploaded
                }

                db.Entry(events).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // Ensure SelectList for BrandID on error
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
