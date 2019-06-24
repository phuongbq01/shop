using Common;
using Facebook;
using Models.DAO;
using Models.EF;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class UserController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        // View Đăng Ký
        // GET: User
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)     // Kiểm tra fomr
            {
                var dao = new UserDAO();
                if (dao.CheckUserName(model.UserName))  // kiểm tra Tên Đăng Nhập
                {
                    ModelState.AddModelError("", "Tên Đăng Nhập Đã Tồn Tại");
                }
                else if (dao.CheckEmail(model.Email))    // Kiểm tra Email
                {
                    ModelState.AddModelError("", "Email Đã Được Đăng Ký");
                }
                else
                {
                    var user = new User();      // tạo mới 1 người dùng
                    user.UserName = model.UserName;
                    user.Password = Encryptor.MD5Hash(model.Password);
                    user.Name = model.Name;
                    user.Address = model.Address;
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.GroupID = "MEMBER";
                    user.CreateDate = DateTime.Now;
                    user.Status = true;
                    var result = dao.Insert(user);  // Thêm người dùng
                    if (result > 0)
                    {
                        UserLogin u = new UserLogin();
                        u.UserID = user.ID;
                        u.UserName = user.UserName;
                        u.Name = user.Name;
                        u.GroupID = user.GroupID;
                        Session[Common.SessionCommonConstants.USER_SESSION] = u;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng Ký Thất Bại");
                    }
                }
            }
            return View(model);
        }


        // Đăng Nhập
        public ActionResult Login()
        {
            return View();
        }


        // 
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDAO();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password), false);

                if (result == 1)    // Nếu tài khoản đúng
                {
                    var user = dao.GetByUserName(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    userSession.Name = user.Name;
                    userSession.GroupID = user.GroupID;
                    var listCredentials = dao.GetListCredentials(user.UserName);

                    Session.Add(Common.SessionCommonConstants.USER_SESSION, userSession);
                    Session.Add(Common.SessionCommonConstants.CREDENTIALS_SESSION, listCredentials);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài Khoản Không Tồn Tại");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài Khoản Đang Bị Khóa");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật Khẩu Không Đúng");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Bạn Không Có Quyền Đăng Nhập");
                }
                else
                {
                    ModelState.AddModelError("", "Lỗi Đăng Nhập");
                }
            }
            return View(model);
        }


        // Đăng Nhập Bằng Facebook
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(
                new
                {
                    client_id = ConfigurationManager.AppSettings["FbAppID"],
                    client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                    redirect_uri = RedirectUri.AbsoluteUri,
                    response_type = "code",
                    scope = "email",
                }
                );
            return Redirect(loginUrl.AbsoluteUri);
        }


        // Kết quả trả về từ Facebook
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppID"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;

                // Lấy Thông Tin
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,email");

                var user = new User();
                user.Email = me.email;
                user.UserName = me.email;
                user.Password = Encryptor.MD5Hash("123456");
                user.GroupID = "MEMBER";
                user.Status = true;
                user.Name = me.first_name + " " + me.middle_name + " " + me.last_name;
                user.CreateDate = DateTime.Now;
                user.CreateBy = user.UserName;

                var resultInsert = new UserDAO().InsertForFacebook(user);   // Thêm User
                if (resultInsert > 0)
                {
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = resultInsert;
                    userSession.GroupID = user.GroupID;
                    userSession.Name = user.Name;
                    Session.Add(Common.SessionCommonConstants.USER_SESSION, userSession);
                }
            }
            return Redirect("/");
        }

        // thông tin người dùng
        [HttpGet]
        public ActionResult UserDetail()
        {
            var userSession = (UserLogin)Session[Common.SessionCommonConstants.USER_SESSION];
            var user = new User();
            if (userSession != null)
            {
                user = new UserDAO().GetByID(userSession.UserID);
            }

            return View(user);
        }

        // Cập nhật thông tin cá nhân của người dùng
        [HttpPost]
        public ActionResult UserDetail(User user)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(user.Password))
                {
                    user.Password = Encryptor.MD5Hash(user.Password);
                }
                user.GroupID = "MEMBER";
                user.Status = true;
                user.ModifiedBy = user.UserName;
                user.ModifiedDate = DateTime.Now;
                var result = new UserDAO().Update(user);
                if (result == true)
                {
                    return RedirectToAction("UserDetail", "User");
                }
            }
            return View(user);
        }


        // Đăng Xuất
        public ActionResult Logout()
        {
            Session[Common.SessionCommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }

    }
}