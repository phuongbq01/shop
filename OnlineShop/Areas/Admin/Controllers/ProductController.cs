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
    public class ProductController : BaseController
    {
        // Danh Sách Sản Phẩm
        // GET: Admin/Product
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            var model = new ProductDAO().ListAll(searchString, page, pageSize);
            return View(model);
        }


        // Thêm Sản Phẩm
        [HttpGet]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(string selectedId = null)
        {
            ViewBag.CategoryID = new SelectList(new ProductCategoryDAO().ListAll(), "ID", "Name", selectedId);
            return View();
        }


        // Thêm Sản Phẩm
        [HttpPost]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var userSession = (UserLogin)Session[Common.SessionCommonConstants.USER_SESSION];
                product.CreateDate = DateTime.Now;
                product.CreateBy = userSession.UserName;
                var result = new ProductDAO().Insert(product);
                if (result > 0)
                {
                    SetAlert("Thêm Sản Phẩm Thành Công", "success");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm Sản Phẩm Thất Bại");
                }
            }
            return View("Index");
        }

        // Edit
        [HttpGet]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(int id, string selectedId)
        {
            var model = new ProductDAO().GetByID(id);
            ViewBag.CategoryID = new SelectList(new ProductCategoryDAO().ListAll(), "ID", "Name", selectedId);
            return View(model);
        }


        //
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var userSession = (UserLogin)Session[Common.SessionCommonConstants.USER_SESSION];
                product.ModifiedBy = userSession.UserName;
                product.ModifiedDate = DateTime.Now;
                var result = new ProductDAO().Edit(product);
                if (result)
                {
                    SetAlert("Cập Nhật Thành Công", "success");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Cập Nhật Thất Bại");
                }
            }
            return View("Index");
        }


        // Delete
        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(long id)
        {
            var result = new ProductDAO().Delete(id);
            if (result)
            {
                SetAlert("Xóa Sản Phẩm Thành Công", "success");
                return RedirectToAction("Index", "Product");
            }
            else
            {
                ModelState.AddModelError("", "Xóa Sản Phẩm Thất Bại");
            }
            return View("Index");
        }


        // ChangeStatus
        [HasCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ProductDAO().ChangeStatus(id);

            return Json(new {
                status = result
            });
        }
    }
}