using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Bạn Chưa Nhập UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Nhập Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}