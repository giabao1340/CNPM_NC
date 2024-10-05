using Mall_Management.Models;
using System.Linq;
using System.Web.Mvc;

public class AccountController : Controller
{
    private mall_dbEntities db = new mall_dbEntities();

    // GET: Registration
    public ActionResult DangKy()
    {
        return View();
    }

    // POST: Registration
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DangKy(Account account, string CFPassword) // Capture confirm password from form
    {
        if (ModelState.IsValid)
        {
            // Check if the username or email already exists
            var existingUser = db.Accounts.FirstOrDefault(a => a.Username == account.Username || a.Email == account.Email);
            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "Username or Email already exists. Please try again.";
                return View(account);
            }

            // Compare plain password and confirm password directly (without BCrypt)
            if (account.PasswordHash != account.CFPassword)
            {
                ViewBag.ErrorMessage = "The password and confirm password do not match. Please try again.";
                return View(account);
            }

            // Encrypt the password before saving using BCrypt
            account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(account.PasswordHash);

            // Save the account to the database
            db.Accounts.Add(account);
            db.SaveChanges();

            // Registration successful, redirect to the homepage
            return RedirectToAction("Index", "Home");
        }

        // If ModelState is invalid, return to registration with failure message
        ViewBag.ErrorMessage = "Registration failed. Please check your input and try again.";
        return View(account);
    }

    public ActionResult DangNhap()
    {
        return View();
    }
}
