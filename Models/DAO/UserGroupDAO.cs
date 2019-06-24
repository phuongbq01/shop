using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class UserGroupDAO
    {
        private OnlineShopDbContext db = null;

        public UserGroupDAO()
        {
            db = new OnlineShopDbContext();
        }

        public List<UserGroup> ListAll()
        {
            return db.UserGroups.Where(x=>x.Status == true).ToList();
        }
    }
}
