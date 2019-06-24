using Common;
using Models.DAO;
using OnlineShop.Areas.Admin.Models;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // Hàm Get đưa ra View Login
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        // Hàm Post nhận Login
        [HttpPost]
        public ActionResult Login(Models.LoginModel model)
        {
            if (ModelState.IsValid)  // Kiểm tra form rỗng
            {
                var dao = new UserDAO();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password), true);
                if (result == 1)    // Nếu tài khoản đúng
                {
                    // Lưu user vào Session
                    var user = dao.GetByUserName(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserID = user.ID;
                    userSession.UserName = user.UserName;
                    userSession.Name = user.Name;
                    userSession.GroupID = user.GroupID;
                    var listCredentials = dao.GetListCredentials(user.UserName);

                    Session.Add(SessionCommonConstants.USER_SESSION, userSession);
                    Session.Add(SessionCommonConstants.CREDENTIALS_SESSION, listCredentials);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài Khoản Không Tồn Tại");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài Khoản Bị Khóa");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật Khẩu Không Đúng");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Bạn Không Có Quyền Đăng Nhập ADMIN");
                }
                else
                {
                    ModelState.AddModelError("", "Lỗi Đăng Nhập");
                }
            }
            return View("Index");
        }
    }
}