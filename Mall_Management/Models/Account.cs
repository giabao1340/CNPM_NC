using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mall_Management.Models
{
    public partial class Account
    {
        public int AccountID { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long, including at least one letter, one number, and one special character.")]
        public string Password { get; set; }  // Plain-text password

        public string PasswordHash { get; set; }  // Hashed password

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string CFPassword { get; set; }

        //[Required(ErrorMessage = "Full Name is required.")]
        //[StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "invalid phone number.")]
        public string Phone { get; set; }

        public string Avatar { get; set; }

        public string Role { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public Nullable<bool> IsActive { get; set; }
    }
}
