using Mall_Management.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using System.Web.Security;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Web;

public class AccountController : Controller
{
    private mall_dbEntities db = new mall_dbEntities();

    public ActionResult DangKy()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DangKy(Account account, string CFPassword)
    {
        if (ModelState.IsValid)
        {
            // Kiểm tra tên đăng nhập và email đã tồn tại
            bool isUsernameExists = db.Accounts.Any(a => a.Username.Equals(account.Username, StringComparison.OrdinalIgnoreCase));
            bool isEmailExists = db.Accounts.Any(a => a.Email.Equals(account.Email, StringComparison.OrdinalIgnoreCase));

            if (isUsernameExists)
            {
                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.");
            }
            if (isEmailExists)
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng. Vui lòng sử dụng email khác.");
            }

            // Nếu có lỗi trùng lặp, trả về trang đăng ký cùng với thông báo lỗi
            if (isUsernameExists || isEmailExists)
            {
                return View(account);
            }

            // Kiểm tra mật khẩu và mật khẩu xác nhận có khớp không
            if (!string.Equals(account.Password, CFPassword))
            {
                ModelState.AddModelError(string.Empty, "Mật khẩu và mật khẩu xác nhận không khớp. Vui lòng thử lại.");
                return View(account);
            }

            // Băm mật khẩu trước khi lưu
            account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(account.Password);
            account.Password = null; // Xóa mật khẩu gốc để bảo mật

            // Thiết lập các thuộc tính mặc định
            account.CreatedDate = DateTime.Now;
            account.Role = "User";  // Vai trò mặc định là "User"
            account.IsActive = true;  // Tài khoản chưa được kích hoạt
            account.FullName = "";
            // Thêm tài khoản vào cơ sở dữ liệu
            db.Accounts.Add(account);
            try
            {
                db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

                // Đăng ký thành công, chuyển hướng đến trang đăng nhập
                TempData["SuccessMessage"] = "Đăng ký thành công. Vui lòng đăng nhập!";
                return RedirectToAction("DangNhap", "Account");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                Console.WriteLine("Validation errors occurred:");
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
                Console.WriteLine("Stack Trace: " + ex.StackTrace);

                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi xác thực dữ liệu. Vui lòng thử lại.");
            }
        }

        // Xử lý lỗi xác thực mô hình khác
        ModelState.AddModelError(string.Empty, "Đăng ký thất bại. Vui lòng kiểm tra và thử lại.");
        return View(account);
    }

    public ActionResult DangNhap()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DangNhap(string username, string password)
    {
        // Tìm tài khoản theo tên đăng nhập
        var account = db.Accounts.SingleOrDefault(a => a.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        if (account == null)
        {
            ModelState.AddModelError("", "Tên đăng nhập không tồn tại.");
            return View();
        }

        // Kiểm tra mật khẩu có khớp không
        if (!BCrypt.Net.BCrypt.Verify(password, account.PasswordHash))
        {
            ModelState.AddModelError("", "Mật khẩu không chính xác.");
            return View();
        }

        // Kiểm tra tài khoản có được kích hoạt không
        if (!account.IsActive)
        {
            ModelState.AddModelError("", "Tài khoản của bạn chưa được kích hoạt.");
            return View();
        }
        Session["AccountID"] = account.AccountID;
        Session["UserName"] = account.Username;
        Session["Role"] = account.Role; // Giả sử tài khoản có thuộc tính Role lưu vai trò
        // Đăng nhập thành công, cập nhật trạng thái IsActive = true
        account.IsActive = true;
        db.SaveChanges();

        // Thiết lập cookie xác thực
        FormsAuthentication.SetAuthCookie(account.Username, false);

        // Chuyển hướng đến trang chủ hoặc trang khác
        return RedirectToAction("Index", "Home");
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DangXuat()
    {
        // Xóa thông tin xác thực
        FormsAuthentication.SignOut();

        // Xóa Session nếu cần
        Session.Clear();

        // Chuyển hướng đến trang đăng nhập hoặc trang chủ
        return RedirectToAction("DangNhap", "Account");
    }





    [HttpGet]
    public ActionResult DoiMatKhau()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DoiMatKhau(PasswordChangeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model); // Return the view with validation errors
        }

        // Fallback to session-based authentication if User.Identity.Name is not populated
        var currentUsername = User.Identity.Name;
        if (string.IsNullOrEmpty(currentUsername))
        {
            currentUsername = Session["Username"]?.ToString(); // Check session if identity name is missing
        }

        if (string.IsNullOrEmpty(currentUsername))
        {
            ModelState.AddModelError(string.Empty, "Không thể tìm thấy tài khoản đang đăng nhập.");
            return View(model);
        }

        var account = await db.Accounts.SingleOrDefaultAsync(a => a.Username == currentUsername);

        if (account == null)
        {
            ModelState.AddModelError(string.Empty, "Tài khoản không tồn tại.");
            return View(model);
        }

        // Validate current password using BCrypt
        if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, account.PasswordHash))
        {
            ModelState.AddModelError(string.Empty, "Mật khẩu hiện tại không đúng.");
            return View(model);
        }

        // Hash the new password using BCrypt before saving it to the database
        account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

        // Save the changes to the database
        db.Entry(account).State = EntityState.Modified;
        await db.SaveChangesAsync();

        TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi thành công!";
        return RedirectToAction("Index", "Home");
    }
}