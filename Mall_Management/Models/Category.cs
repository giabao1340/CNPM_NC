using Mall_Management.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public partial class Category
{
    public int CategoryID { get; set; }

    [Required(ErrorMessage = "Category Name is required")]
    [StringLength(100, ErrorMessage = "Category Name cannot be longer than 100 characters.")]
    public string CategoryName { get; set; }

    public virtual ICollection<Brand> Brands { get; set; }
}
