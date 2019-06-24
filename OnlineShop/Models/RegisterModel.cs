using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class RegisterModel
    {
        [Key]
        public long ID { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Nhập Tên Đăng Nhập")]
        [Display(Name = "Tên Đăng Nhập")]
        [StringLength(50,  MinimumLength = 3, ErrorMessage = "Tên Đăng Nhập Dài Từ 3 - 50 Ký Tự")]
        public string UserName { get; set; }

        [Required(ErrorMessage = " Bạn Chưa Nhập Mật Khẩu")]
        [Display(Name = "Mật Khẩu")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Mật Khẩu Dài Từ 6 - 32 Ký Tự")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Nhập Mật Khẩu")]
        [Display(Name = "Xác Nhận Mật Khẩu")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Mật Khẩu Dài Từ 6 - 32 Ký Tự")]
        [Compare("Password", ErrorMessage = "Mật Khẩu Xác Nhận Không Đúng")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Nhập Họ Tên")]
        [Display(Name = "Họ Tên")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Tên Dài Từ 1 - 50 Ký Tự")]
        public string Name { get; set; }

        [Required(ErrorMessage = " Bạn Chưa Nhập Địa Chỉ")]
        [Display(Name = "Địa Chỉ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Nhập Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Số Điện Thoại")]
        public string Phone { get; set; }
    }
}