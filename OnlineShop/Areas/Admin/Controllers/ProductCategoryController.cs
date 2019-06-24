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
    public class ProductCategoryController : BaseController
    {
        // Trang danh sách Danh Mục
        // GET: Admin/ProductCategory
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 2)
        {
            var model = new ProductCategoryDAO().ListAllPaging(searchString, page, pageSize);
            return View(model);
        }


        // Action trả về View thêm Danh Mục
        [HttpGet]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            return View();
        }


        // Action thêm Danh Mục Sản Phẩm
        [HttpPost]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(ProductCategory category)
        {
            if (ModelState.IsValid)
            {
                var userSession = (UserLogin)Session[Common.SessionCommonConstants.USER_SESSION];
                category.CreateDate = DateTime.Now;
                category.CreateBy = userSession.UserName;
                var result = new ProductCategoryDAO().Insert(category);
                if (result > 0)
                {
                    SetAlert("Thêm Danh Mục Sản Phẩm Thành Công", "success");    // Hàm kế thừa từ BaseController
                    return RedirectToAction("Index", "ProductCategory");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm Danh Mục Sản Phẩm Thất Bại");
                }
            }
            return View("Index");
        }


        // Action Delete
        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(long id)
        {
            var result = new ProductCategoryDAO().Delete(id);
            if (result)
            {
                SetAlert("Xóa Thành Công", "success");
                return RedirectToAction("Index");
            }
            else
                ModelState.AddModelError("", "Xóa Thất Bại");
            return View("Index");
        }


        // Hàm thay đổi trạng thái hoạt động
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ProductCategoryDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}