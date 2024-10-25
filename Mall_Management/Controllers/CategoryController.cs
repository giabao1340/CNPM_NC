using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Mall_Management.Models;  // Adjust the namespace for your models 

namespace Mall_Management.Controllers
{
    public class CategoryController : Controller { 
        private mall_dbEntities _db = new mall_dbEntities();  // Entity Framework DbContext
        // GET: Category
        public ActionResult Index()
        {
            // Fetch all categories from the database
            var categories = _db.Categories.ToList();

            // Ensure 'categories' is not null
            if (categories == null)
            {
                categories = new List<Category>();  // Initialize to avoid null reference
            }

            return View(categories);  // Pass the list of categories to the view
        }


        //public ActionResult Details(int id)
        //{
        //    // Fetch the category from the database
        //    var category = _db.Categories.FirstOrDefault(c => c.CategoryID == id);

        //    // Check if category is null (not found in the database)
        //    if (category == null)
        //    {
        //        return HttpNotFound();  // Or you can redirect to an error page
        //    }

        //    // Pass the category to the view
        //    return View(category);
        //}


        // GET: Category/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();  // Just return the empty form
        }

        public ActionResult Create(Category category)
        {
            // Kiểm tra tên Category có trùng không
            var existingCategory = _db.Categories.FirstOrDefault(c => c.CategoryName == category.CategoryName);

            if (existingCategory != null)
            {
                // Nếu tên Category đã tồn tại, thêm thông báo lỗi
                ModelState.AddModelError("CategoryName", "Category Name already exists.");
            }

            if (ModelState.IsValid)  // Validate the model
            {
                _db.Categories.Add(category);  // Add the new category to the DbSet
                _db.SaveChanges();  // Commit the changes to the database

                return RedirectToAction("Index");  // Redirect back to the category list
            }

            return View(category);  // If validation fails, redisplay the form with the entered data
        }



        public ActionResult Edit(int id)
        {
            // Find the category by id
            var category = _db.Categories.FirstOrDefault(c => c.CategoryID == id);

            // If no category is found, return 404
            if (category == null)
            {
                return HttpNotFound();
            }

            // Pass the category to the view
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]  // Protect against CSRF
        public ActionResult Edit(int id, Category category)
        {
            // Kiểm tra tên Category có trùng không (trừ trường hợp cùng một ID)
            var existingCategory = _db.Categories.FirstOrDefault(c => c.CategoryName == category.CategoryName && c.CategoryID != id);

            if (existingCategory != null)
            {
                // Nếu tên Category đã tồn tại, thêm thông báo lỗi
                ModelState.AddModelError("CategoryName", "Category Name already exists.");
            }

            if (ModelState.IsValid)  // Validate the model
            {
                // Fetch the category from the database
                var categoryToUpdate = _db.Categories.FirstOrDefault(c => c.CategoryID == id);

                // If category exists, update its properties
                if (categoryToUpdate != null)
                {
                    categoryToUpdate.CategoryName = category.CategoryName;

                    // Save changes to the database
                    _db.SaveChanges();

                    // Redirect to the Index page after successful update
                    return RedirectToAction("Index");
                }

                return HttpNotFound();  // If the category was not found in the database
            }

            // If validation fails, return the same view with the model data
            return View(category);
        }


        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var category = _db.Categories.Find(id);
                if (category == null)
                {
                    return Json(new { success = false, message = "Thương hiệu không tồn tại." });
                }

                _db.Categories.Remove(category);
                _db.SaveChanges();

                return Json(new { success = true, message = "Xóa thành công." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}
