using Mall_Management.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            var events = db.Events.ToList();
            return View(events);
        }
        public ActionResult Brands()
        {
            var brands = db.Brands.ToList();
            return View(brands);
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
            var spaces = db.Spaces.ToList();
            return View(spaces);
        }
        public ActionResult Map()
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

        [HttpPost]
        public async Task<JsonResult> RentNow(int spaceId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để thuê mặt bằng." });
            }

            try
            {
                var username = User.Identity.Name;
                var user = await db.Accounts.SingleOrDefaultAsync(a => a.Username == username);


                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng." });
                }

                db.Entry(user).State = System.Data.Entity.EntityState.Modified;

                // Kiểm tra xem mặt bằng có đang khả dụng không
                var space = await db.Spaces.FindAsync(spaceId);
                if (space == null || space.Status != "Available")
                {
                    return Json(new { success = false, message = "Mặt bằng không khả dụng." });
                }

                // Tạo hợp đồng thuê mới
                var contract = new Contract
                {
                    AccountID = user.AccountID,
                    SpaceID = space.SpaceID,
                    StartDate = DateTime.Now,
                    RentAmount = space.RentalPrice,
                    Status = "Pending"
                };
                db.Contracts.Add(contract);

                // Cập nhật trạng thái của mặt bằng thành "Rented"
                space.Status = "Rented";
                db.Entry(space).State = System.Data.Entity.EntityState.Modified;

                // Lưu các thay đổi vào cơ sở dữ liệu
                await db.SaveChangesAsync();

                return Json(new { success = true, message = "Thuê mặt bằng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }

    }
}