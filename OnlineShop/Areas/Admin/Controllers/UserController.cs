using Common;
using Models.DAO;
using Models.EF;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // Trang danh sách User
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string stringSearch, int page = 1, int pageSize = 4)
        {
            var model = new UserDAO().ListAllPaging(stringSearch, page, pageSize);
            return View(model);
        }

        // Trang Thêm Mới User
        // GET: Admin/User
        [HttpGet]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }


        // Action thêm mới User
        [HttpPost]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid) 
            { 
                var dao = new UserDAO();
                if (dao.CheckUserName(user.UserName))   // Đã Tồn Tại UserName
                {
                    ModelState.AddModelError("", "UserName Đã Tồn Tại");
                }
                else if (dao.CheckEmail(user.Email))
                {
                    ModelState.AddModelError("", "Email Đã Được Đăng Ký");
                }
                else
                {
                    var userSession = (UserLogin)Session[Common.SessionCommonConstants.USER_SESSION];
                    user.Password = Encryptor.MD5Hash(user.Password);
                    user.CreateDate = DateTime.Now;
                    user.CreateBy = user.UserName;
                    if (user.GroupID == null)
                        user.GroupID = "MEMBER";
                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        SetAlert("Thêm User Thành Công", "success");    // Hàm kế thừa từ BaseController
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm Thất Bại");
                    }
                }
            }
            SetViewBag();
            return View(user);
        }


        // 
        public void SetViewBag(string selectedId = null)
        {
            var dao = new UserGroupDAO();
            ViewBag.GroupID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }


        // Action trả về View Update
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(long id)
        {
            var model = new UserDAO().GetByID(id);
            SetViewBag(model.GroupID);
            return View(model);
        }

        // Action Update
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var userSession = (UserLogin)Session[SessionCommonConstants.USER_SESSION];

                if (!string.IsNullOrEmpty(user.Password))
                {
                    User model = new UserDAO().GetByID(user.ID);
                    user.Password = Encryptor.MD5Hash(model.Password);
                }
                user.ModifiedBy = userSession.UserName;
                user.ModifiedDate = DateTime.Now;
                var result = new UserDAO().Update(user);
                if (result)
                {
                    SetAlert("Edit User Thành Công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập Nhật Thất Bại");
                }

            }
            SetViewBag();
            return View("Edit");
        }


        // Action Delete
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(long id)
        {
            var result = new UserDAO().Delete(id);
            if (result)
            {
                SetAlert("Xóa User Thành Công", "success");
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Xóa Thất Bại");
            }
            return View("Index");
        }


        // Thay đổi trạng thái hoạt động của user
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(long id)
        {
            var result = new UserDAO().ChangeStatus(id);
            return Json(new {
                status = result
            });
        }
    }
}