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
                // Send a JSON response indicating that the user should be redirected to login
                return Json(new { success = false, redirectUrl = Url.Action("Login", "Account"), message = "Bạn cần đăng nhập để thuê mặt bằng." });
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
        public ActionResult History()
        {
            // Lấy tên người dùng hiện tại từ Forms Authentication
            string username = User.Identity.Name;

            // Truy xuất AccountID từ bảng Accounts dựa trên UserName
            var userAccount = db.Accounts.FirstOrDefault(a => a.Username == username);

            if (userAccount != null)
            {
                int userId = userAccount.AccountID;

                // Lấy danh sách hợp đồng của người dùng
                var contracts = db.Contracts
                                  .Include(c => c.Space)
                                  .Where(c => c.AccountID == userId)
                                  .OrderByDescending(c => c.StartDate)
                                  .ToList();

                return View(contracts);
            }

            // Nếu không tìm thấy người dùng, trả về view rỗng
            return View(new List<Contract>());
        }
        public ActionResult Infor()
        {
            // Lấy UserId từ Session (ví dụ nếu lưu trong session)
            int? userId = Session["AccountID"] as int?;
            if (userId == null)
            {
                return RedirectToAction("DangNhap", "Account"); // Điều hướng nếu chưa đăng nhập
            }

            var account = db.Accounts.SingleOrDefault(a => a.AccountID == userId.Value);
            if (account == null)
            {
                return HttpNotFound("Account not found.");
            }

            return View(account);
        }
        public ActionResult Edit(int id)
        {
            var account = db.Accounts.SingleOrDefault(a => a.AccountID == id);
            if (account == null)
            {
                return HttpNotFound("Account not found.");
            }
            return View(account);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Infor", new { id = account.AccountID });
            }
            return View(account);
        }

    }
}