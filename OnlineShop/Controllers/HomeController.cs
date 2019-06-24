using Models.DAO;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var prodDAO = new ProductDAO();
            ViewBag.Slides = new SlideDAO().ListAll();
            ViewBag.NewProducts = prodDAO.ListNewProducts(4);
            ViewBag.HotProducts = prodDAO.ListHotProducts(4);
            return View();
        }


        // 
        [ChildActionOnly]
        [OutputCache(Duration = 3600 * 24)]     // cache trong 1 ngày
        public ActionResult MainMenu()
        {
            var model = new MenuDAO().ListByGroupID(1);
            return PartialView(model);
        }


        //
        [ChildActionOnly]
        public ActionResult MenuTop()
        {
            var user = (UserLogin)Session[Common.SessionCommonConstants.USER_SESSION];
            if (user != null)
                ViewBag.user = user;
            var model = new MenuDAO().ListByGroupID(2);
            return PartialView(model);
        }


        // Cart
        [ChildActionOnly]
        public ActionResult HeaderCart()
        {
            var cartSession = Session[Common.SessionCommonConstants.CART_SESSION];
            var model = new List<CartItem>();
            if (cartSession != null)
                model = (List<CartItem>) cartSession;
            decimal? total = 0;
            foreach (var item in model)
                total += ((item.Product.PromotionPrice != null ? item.Product.PromotionPrice : item.Product.Price) * item.Quantity);
            ViewBag.total = total;
            return PartialView(model);
        }


        // 
        [OutputCache(Duration = 3600 * 24)]
        public ActionResult Footer()
        {
            var model = new FooterDAO().GetFooter();
            return PartialView(model);
        }
    }
}