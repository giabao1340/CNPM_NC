using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mall_Management.Models
{
    public partial class Brand
    {
        public int BrandID { get; set; }

        [Required(ErrorMessage = "Tên thương hiệu là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên thương hiệu không được vượt quá 100 ký tự")]
        public string BrandName { get; set; }

        [Url(ErrorMessage = "Đường dẫn URL không hợp lệ")]
        public string Url { get; set; }

        public string Image { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Tầng là bắt buộc")]
        public string Floor { get; set; }

        public Nullable<int> CategoryID { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Promotion> Promotions { get; set; }
        public virtual ICollection<Space> Spaces { get; set; }
        public virtual ICollection<Staff> Staffs { get; set; }
    }
}
