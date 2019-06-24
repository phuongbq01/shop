using Common;
using Models.DAO;
using Models.EF;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {
        // Danh Sách Sản Phẩm Trong Giỏ Hàng
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[Common.SessionCommonConstants.CART_SESSION];     // Lấy giỏ hàng trong Session
            var list = new List<CartItem>();
            if (cart != null)
                list = (List<CartItem>)cart;
            decimal? total = 0;
            foreach (var item in list)
                total += ((item.Product.PromotionPrice != null ? item.Product.PromotionPrice : item.Product.Price) * item.Quantity);
            ViewBag.total = total;
            return View(list);
        }


        // Thêm sản phẩm vào giỏ hàng
        public ActionResult AddItem(long id, int quantity)
        {
            var product = new ProductDAO().GetByID(id);

            var cart = Session[Common.SessionCommonConstants.CART_SESSION];
            if(cart != null)    // Nếu đã có giỏ hàng
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x=>x.Product.ID == id))     // Nếu giỏ hàng đã có sản phẩm này
                {
                    foreach (var item in list)              // Tìm sản phẩm đó trong giỏ hàng
                    {
                        if (item.Product.ID == id)
                        {
                            item.Quantity += quantity;      // Tăng số lượng
                        }
                    }
                }
                else    // Nếu trong giỏ hàng chưa có sản phẩm này
                {
                    var item = new CartItem();      // Thêm mới sản phẩm vào giỏ hàng
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                Session[Common.SessionCommonConstants.CART_SESSION] = list;     // Cập nhật lại giỏ hàng trong Session
            }
            else        // Nếu chưa có giỏ hàng
            {
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();    // Tạo ra 1 danh sách lưu các CartItem
                list.Add(item);                     // Thêm sản phẩm vào giỏ hàng

                Session[Common.SessionCommonConstants.CART_SESSION] = list;     // Cập nhật lại giỏ hàng
            }
            return RedirectToAction("Index", "Home");
        }


        // Update giỏ hàng
        public ActionResult Update(string cartModel)    // giá trị truyền vào là 1 chuỗi JSON
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);   // Mã hóa chuỗi cartModel truyền vào
            var sessionCart = (List<CartItem>)Session[Common.SessionCommonConstants.CART_SESSION];  // Lấy giỏ hàng từ Session

            foreach (var item in sessionCart)   // duyệt giỏ hàng
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if ( jsonItem != null)   // nếu sản phẩm item trong giỏ hàng trùng với sản phẩm trong jsonCart
                {
                    item.Quantity = jsonItem.Quantity;      // Cập nhật lại số lượng
                }
            }

            Session[Common.SessionCommonConstants.CART_SESSION] = sessionCart;      // Cập nhật lại giỏ hàng

            return Json(new
            {
                status = true
            });
        }


        // Xóa 1 sản phẩm trong giỏ hàng
        public ActionResult Delete(long id)
        {
            var list = (List<CartItem>)Session[Common.SessionCommonConstants.CART_SESSION];     // Lấy giỏ hàng trong Session
            list.RemoveAll (x=>x.Product.ID == id);     // Xóa sản phẩm
            Session[Common.SessionCommonConstants.CART_SESSION] = list;     // Cập nhật lại giỏ hàng

            return Json(new {
                status = true
            });
        }

        // Xóa Giỏ Hàng
        public ActionResult DeleteAll()
        {
            Session[Common.SessionCommonConstants.CART_SESSION] = null;

            return Json(new {
                status = true
            });
        }


        // Thanh toán
        [HttpGet]
        public ActionResult Payment()
        {
            var userSession = (UserLogin)Session[Common.SessionCommonConstants.USER_SESSION];
            if (userSession != null)
            {
                var user = new UserDAO().GetByID(userSession.UserID);
                ViewBag.user = user;
            }
            var cartSession = (List<CartItem>)Session[Common.SessionCommonConstants.CART_SESSION];
            var model = new List<CartItem>();
            if (cartSession != null)
            {
                model = cartSession;
            }

            decimal? total = 0;
            foreach (var item in model)
                total += ((item.Product.PromotionPrice != null ? item.Product.PromotionPrice : item.Product.Price) * item.Quantity);
            ViewBag.total = total;

            return View(model);
        }


        //
        [HttpPost]
        public ActionResult Payment(string shipName, string mobile, string address, string email)
        {
            var order = new Order();    // Tạo 1 đơn hàng
            order.CreateDate = DateTime.Now;
            order.ShipName = shipName;
            order.ShipMobile = mobile;
            order.ShipAddress = address;
            order.ShipEmail = email;

            try {
                var id = new OrderDAO().Insert(order);  // Thêm mới 1 đơn hàng
                var cartSession = (List<CartItem>)Session[Common.SessionCommonConstants.CART_SESSION];      // Lấy danh sách giỏ hàng từ Session
                var detailDAO = new OrderDetailDAO();
                decimal? total = 0;

                foreach (var item in cartSession)
                {
                    var detail = new OrderDetail();
                    detail.ProductID = item.Product.ID;
                    detail.OrderID = id;
                    detail.Quantity = item.Quantity;
                    detail.Price = (item.Product.PromotionPrice != null ? item.Product.PromotionPrice : item.Product.Price);
                    detailDAO.Insert(detail);       // Thêm từng sản phẩm trong đơn hàng

                    total += (detail.Price * detail.Quantity);  // Tính tổng tiền
                }

                // Đọc file nội dung Email
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/Client/template/neworder.html"));
                content = content.Replace("{{CustomerName}}", shipName);
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Total}}", total.GetValueOrDefault(0).ToString("N0"));

                // Địa chỉ Email quản trị
                var toEmailAddress = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                // Gửi Email
                new EmailHelper().SendEmail(email, "Đơn Hàng Mới Từ OnlineShop", content);  // Gửi Email cho khách mua hàng
                new EmailHelper().SendEmail(toEmailAddress, "Đơn Hàng Mới Từ OnlineShop", content);     // Gửi Email cho quản trị
            }
            catch(Exception ex)
            {
                return Redirect("/loi-thanh-toan");
            }
            return Redirect("/hoan-thanh");
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}