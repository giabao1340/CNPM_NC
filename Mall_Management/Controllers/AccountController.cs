using Mall_Management.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using System.Web.Security;
using System.Data.Entity.Validation;
using System.Data.Entity; 

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
            // Check if username or email already exists
            var existingUser = db.Accounts.FirstOrDefault(a => a.Username == account.Username || a.Email == account.Email);
            if (existingUser != null)
            {
                // Use ModelState.AddModelError to display the error on the page near the input field
                ModelState.AddModelError(string.Empty, "Username or Email already exists. Please try again.");
                return View(account);
            }

            // Check if password and confirm password match
            if (account.Password != CFPassword)  // Compare plain-text password
            {
                ModelState.AddModelError(string.Empty, "The password and confirm password do not match. Please try again.");
                return View(account);
            }

            // Hash the password using BCrypt before saving to the database
            account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(account.Password);

            // Additional properties
            account.Role = "User";  // Default role for new users
            account.CreatedDate = DateTime.Now;  // Store the current date/time
            // Add the new account to the database
            db.Accounts.Add(account);

            try
            {
                // Save the changes to the database
                db.SaveChanges();
                return RedirectToAction("DangNhap", "Account");
            }
            catch (DbUpdateException ex)
            {
                // Log the detailed exception message (you can extend this by using a logger)
                var errorMessage = ex.InnerException?.Message;
                Console.WriteLine(errorMessage);  // Replace with logging in a real-world app
                ModelState.AddModelError(string.Empty, "An error occurred while saving to the database. Please try again.");
                return View(account);
            }

        }

        // If we get here, something went wrong with model validation
        ModelState.AddModelError(string.Empty, "Registration failed. Please check your input and try again.");
        return View(account);
    }


    public ActionResult DangNhap()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DangNhap(string UsernameOrEmail, string Password)
    {
        if (string.IsNullOrEmpty(UsernameOrEmail) || string.IsNullOrEmpty(Password))
        {
            ModelState.AddModelError(string.Empty, "Username/Email and Password are required.");
            return View();
        }
        var account = db.Accounts.FirstOrDefault(a => a.Username == UsernameOrEmail || a.Email == UsernameOrEmail);

        if (account != null)
        {
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(Password, account.PasswordHash);

            if (isPasswordValid)
            {
                FormsAuthentication.SetAuthCookie(account.Username, false);

                Session["Username"] = account.Username;
                Session["Role"] = account.Role;
                Session["IsActive"] = account.IsActive;

                account.IsActive = true;

                db.Entry(account).State = EntityState.Modified; // Ensure the entity is marked as modified
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid password. Please try again.");
            }
        }
        else
        {
            ModelState.AddModelError(string.Empty, "User not found. Please try again.");
        }

        return View();
    }





    public ActionResult DangXuat()
    {
        var username = Session["Username"];
        if (username != null)
        {
            var account = db.Accounts.FirstOrDefault(a => a.Username == (string)username);

            if (account != null)
            {
                account.IsActive = false;

                db.SaveChanges();
            }

            Session.Clear();
            FormsAuthentication.SignOut();
        }
        return RedirectToAction("DangNhap", "Account");
    }

}
