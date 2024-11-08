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
    public class SpacesController : Controller
    {
        private mall_dbEntities db = new mall_dbEntities();

        // GET: Spaces
        public async Task<ActionResult> Index()
        {
            return View(await db.Spaces.ToListAsync());
        }

        // GET: Spaces/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Space space = await db.Spaces.FindAsync(id);
            if (space == null)
            {
                return HttpNotFound();
            }
            return View(space);
        }

        // GET: Spaces/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SpaceID,SpaceName,Location,Image,Area,RentalPrice,Status")] Space space, HttpPostedFileBase image)
        {
            // Kiểm tra nếu tên thương hiệu đã tồn tại
            var existingSpace = db.Spaces.FirstOrDefault(b => b.SpaceName == space.SpaceName);
            if (existingSpace != null)
            {
                ModelState.AddModelError("SpaceName", "Thương hiệu đã tồn tại.");
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
                    space.Image = "/images/" + fileName;
                }

                // Lưu đối tượng Brand vào cơ sở dữ liệu
                db.Spaces.Add(space);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(space);
        }


        // GET: Spaces/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Space space = await db.Spaces.FindAsync(id);
            if (space == null)
            {
                return HttpNotFound();
            }
            return View(space);
        }
        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SpaceID,SpaceName,Location,Image,Area,RentalPrice,Status")] Space space, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu tên sự kiện đã tồn tại, bỏ qua sự kiện hiện tại
                var existingSpace = db.Spaces.FirstOrDefault(b => b.SpaceName == space.SpaceName && b.SpaceID != space.SpaceID);
                if (existingSpace != null)
                {
                    ModelState.AddModelError("SpaceName", "Mat bang đã tồn tại.");
                    return View(space);
                }

                // Xử lý ảnh tải lên
                if (image != null && image.ContentLength > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/"), fileName);
                    image.SaveAs(path);
                    space.Image = "/images/" + fileName;
                }
                else
                {
                    space.Image = db.Spaces.AsNoTracking().Where(b => b.SpaceID == space.SpaceID).Select(b => b.Image).FirstOrDefault();
                }

                db.Entry(space).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(space);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                // Tìm kiếm space theo ID
                var space = db.Spaces.Find(id);
                if (space == null)
                {
                    return Json(new { success = false, message = "Mặt bằng không tồn tại." });
                }

                // Xóa space khỏi cơ sở dữ liệu
                db.Spaces.Remove(space);
                db.SaveChanges();

                // Trả về kết quả thành công
                return Json(new { success = true, message = "Xóa thành công." });
            }
            catch (Exception ex)
            {
                // Trả về lỗi nếu gặp sự cố
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
