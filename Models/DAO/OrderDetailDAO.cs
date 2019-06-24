using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class OrderDetailDAO
    {
        private OnlineShopDbContext db = null;

        public OrderDetailDAO()
        {
            db = new OnlineShopDbContext();
        }

        public bool Insert(OrderDetail detail)
        {
            try {
                db.OrderDetails.Add(detail);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
