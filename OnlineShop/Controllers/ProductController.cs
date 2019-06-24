using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string stringSearch, int page = 1, int pageSize = 4)
        {
            var model = new ProductDAO().ListSearch(stringSearch, page, pageSize);
            ViewBag.Slides = new SlideDAO().ListAll();
            ViewBag.stringSearch = stringSearch;
            return View(model);
        }


        //Danh sách sản phẩm thuộc 1 danh mục
        public ActionResult Category(long id, int page = 1, int pageSize = 4)
        {
            var model = new ProductDAO().GetByCategoryID(id, page, pageSize);
            ViewBag.Category = new ProductCategoryDAO().GetByID(id);
            return View(model);
        }


        // Chi tiết sản phẩm
        [OutputCache(Duration = 3600 * 24, VaryByParam = "id", Location = System.Web.UI.OutputCacheLocation.Server)]
        public ActionResult Detail(long id)
        {
            var model = new ProductDAO().GetByID(id);
            var category = new ProductCategoryDAO().GetByID(model.CategoryID.Value);
            ViewBag.category = category;
            ViewBag.RelatedProducts = new ProductDAO().List(category.ID);
            return View(model);
        }
    }
}