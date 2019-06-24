using Models.DAO;
using Models.EF;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            var userSession = (UserLogin)Session[Common.SessionCommonConstants.USER_SESSION];
            if (userSession != null)    // Nếu đã đăng nhập
            {
                var user = new UserDAO().GetByID(userSession.UserID);
                ViewBag.user = user;
            }
            var model = new ContactDAO().GetContact();
            return View(model);
        }


        // action Gửi Phản Hồi
        [HttpPost]
        public JsonResult Send(string name, string mobile, string address, string email,string content)
        {
            Feedback fb = new Feedback();
            fb.Name = name;
            fb.Phone = mobile;
            fb.Address = address;
            fb.Email = email;
            fb.Content = content;
            fb.CreateDate = DateTime.Now;

            var result = new FeedbackDAO().Insert(fb);
            if (result > 0)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
    }
}