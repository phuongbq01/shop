using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class LoginModel
    {
        [Key]
        [Required(ErrorMessage = "Mời Bạn Nhập Tên Đăng Nhập")]
        [Display(Name = "Tên Đăng Nhập")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mời Bạn Nhập Mật Khẩu")]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; }
    }
}