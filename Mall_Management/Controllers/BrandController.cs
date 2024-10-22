using Mall_Management.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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


        // POST: Create a new brand
        [HttpPost]
        public JsonResult Create(Brand brand, HttpPostedFileBase image)
        {
            try
            {
                // Validate that BrandName and Floor are not empty
                if (string.IsNullOrEmpty(brand.BrandName) || string.IsNullOrEmpty(brand.Floor))
                {
                    return Json(new { success = false, message = "Brand name and floor are required." }, JsonRequestBehavior.AllowGet);
                }

                // Handle image upload (if an image is provided)
                if (image != null && image.ContentLength > 0)
                {
                    // Get the file name and save it to the "Images/Brands" folder
                    var fileName = Path.GetFileName(image.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/"), fileName);
                    image.SaveAs(path);

                    // Update the brand's image path to store the correct relative URL
                    brand.Image = "/images/" + fileName;
                }

                // Add the new brand to the database
                _db.Brands.Add(brand);
                _db.SaveChanges();

                return Json(new { success = true, message = "Brand created successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Return detailed error for debugging
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public JsonResult Edit(Brand brand)
        {
            string result = "error";
            try
            {
                // Tìm thương hiệu cần sửa
                Brand brandToUpdate = _db.Brands.FirstOrDefault(m => m.BrandID == brand.BrandID);

                // Nếu không tìm thấy thương hiệu
                if (brandToUpdate == null)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                // Kiểm tra nếu tên thương hiệu đã tồn tại cho một thương hiệu khác, bỏ qua thương hiệu hiện tại
                var checkExist = _db.Brands.FirstOrDefault(m => m.BrandName == brand.BrandName && m.BrandID != brand.BrandID);
                if (checkExist != null)
                {
                    result = "exist";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                // Cập nhật thông tin thương hiệu
                brandToUpdate.BrandName = brand.BrandName;
                brandToUpdate.Image = brand.Image;
                brandToUpdate.Floor = brand.Floor;
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
                Console.WriteLine(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public JsonResult Delete(int id)
        {
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
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // Log lỗi nếu cần
                return Json(new { success = false, message = "Error occurred while deleting the brand" }, JsonRequestBehavior.AllowGet);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }

        [HttpPost]
        public JsonResult UploadImage()
        {
            try
            {
                // Kiểm tra xem có file nào trong request không
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        // Đường dẫn đến thư mục lưu file (~/images)
                        var folderPath = Server.MapPath("~/images");

                        // Kiểm tra nếu thư mục ~/images chưa tồn tại, thì tạo mới thư mục
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        // Tạo tên file và đường dẫn đầy đủ để lưu file
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(folderPath, fileName);

                        // Lưu file vào thư mục ~/images
                        file.SaveAs(filePath);

                        // Trả về đường dẫn của file để client hiển thị ảnh
                        return Json(new { filePath = "/images/" + fileName });
                    }
                    else
                    {
                        return Json(new { error = "File không hợp lệ hoặc rỗng." });
                    }
                }

                return Json(new { error = "Không tìm thấy file trong request." });
            }
            catch (Exception ex)
            {
                return Json(new { error = "Đã có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}