using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class MenuDAO
    {
        private OnlineShopDbContext db = null;

        public MenuDAO()
        {
            db = new OnlineShopDbContext();
        }

        public List<Menu> ListByGroupID(int id)
        {
            return db.Menus.Where(x=>x.TypeID == id).ToList();
        }
    }
}
