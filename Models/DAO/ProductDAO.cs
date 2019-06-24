using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Models.DAO
{
    public class ProductDAO
    {
        OnlineShopDbContext db = null;

        public ProductDAO()
        {
            db = new OnlineShopDbContext();
        }


        public long Insert(Product product)
        {
            if (product != null)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return product.ID;
            }
            else
                return -1;
        }



        // Edit
        public bool Edit(Product product)
        {
            try {
                var prod = db.Products.SingleOrDefault(x => x.ID == product.ID);
                if (prod != null)
                {
                    prod.Name = product.Name;
                    prod.Code = product.Code;
                    prod.MetaTitle = product.MetaTitle;
                    prod.Description = product.Description;
                    prod.Image = product.Image;
                    prod.MoreImages = product.MoreImages;
                    prod.Price = product.Price;
                    prod.PromotionPrice = product.PromotionPrice;
                    prod.IncludeVAT = product.IncludeVAT;
                    prod.Quantity = product.Quantity;
                    prod.CategoryID = product.CategoryID;
                    prod.Detail = product.Detail;
                    prod.Warranty = product.Warranty;
                    prod.ModifiedDate = product.ModifiedDate;
                    prod.ModifiedBy = product.ModifiedBy;
                    prod.MetaKeywords = product.MetaKeywords;
                    prod.MetaDescription = product.MetaDescription;
                    prod.Status = product.Status;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Delete
        public bool Delete(long id)
        {
            var product = db.Products.SingleOrDefault(x => x.ID == id);
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
                return true;
            }
            else
                return false;
        }


        // Lấy ra sản phẩm dựa vào ID
        public Product GetByID(long id)
        {
            return db.Products.SingleOrDefault(x => x.ID == id);
        }


        //
        public List<Product> List(long id)
        {
            return db.Products.Where(x=>x.CategoryID == id).Take(4).ToList();
        }


        // Lấy ra danh sách sản phẩm theo danh mục
        public IEnumerable<Product> GetByCategoryID(long id, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            model = model.Where(x => x.CategoryID == id);
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }


        // Lấy ra danh sách Sản Phẩm
        public IEnumerable<Product> ListAll(string searchString, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Code.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }


        // Lấy ra danh sách sản phẩm mới
        public List<Product> ListNewProducts(int count)
        {
            return db.Products.OrderByDescending(x => x.CreateDate).Take(count).ToList();
        }


        // Lấy ra danh sách sản phẩm có lượt xem cao nhất
        public List<Product> ListHotProducts(int count)
        {
            return db.Products.OrderByDescending(x => x.ViewCount).Take(count).ToList();
        }


        public IEnumerable<Product> ListSearch(string stringSearch, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(stringSearch))
            {
                model = model.Where(x=>x.Name.Contains(stringSearch) || x.Code.Contains(stringSearch));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }

        // Change Status
        public bool ChangeStatus(long id)
        {
            var product = db.Products.SingleOrDefault(x=>x.ID == id);
            product.Status = !product.Status;
            db.SaveChanges();
            return product.Status;
        }
    }
}
