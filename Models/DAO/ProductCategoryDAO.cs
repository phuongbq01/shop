using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Models.DAO
{
    public class ProductCategoryDAO
    {
        private OnlineShopDbContext db = null;

        public ProductCategoryDAO()
        {
            db = new OnlineShopDbContext();
        }

        // hàm insert
        public long Insert(ProductCategory category)
        {
            if (category != null)
            {
                db.ProductCategories.Add(category);
                db.SaveChanges();
                return category.ID;
            }
            return -1;
        }


        // Delete
        public bool Delete(long id)
        {
            try {
                var category = db.ProductCategories.SingleOrDefault(x=>x.ID == id);
                if (category != null)
                {
                    db.ProductCategories.Remove(category);
                    db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        // Lấy ra danh mục theo ID
        public ProductCategory GetByID(long id)
        {
            return db.ProductCategories.SingleOrDefault(x=>x.ID == id);
        }


        // 
        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x=>x.Status == true).ToList();
        }

        // Hàm lấy ra toàn bộ Danh Mục kèm Search
        public IEnumerable<ProductCategory> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<ProductCategory> model = db.ProductCategories;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.ID.ToString().Equals(searchString));
            }
            return model.OrderBy(x => x.CreateDate).ToPagedList(page, pageSize);
        }


        // Hàm thay đổi trạng thái hoạt động
        public bool ChangeStatus(long id)
        {
            var category = db.ProductCategories.SingleOrDefault(x=>x.ID == id);
            category.Status = !category.Status;
            db.SaveChanges();
            return category.Status;
        }

    }
}
