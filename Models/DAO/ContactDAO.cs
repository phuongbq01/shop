using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ContactDAO
    {
        private OnlineShopDbContext db = null;

        public ContactDAO()
        {
            db = new OnlineShopDbContext();
        }

        public Contact GetContact()
        {
            return db.Contacts.SingleOrDefault(x=>x.Status == true);
        }
    }
}
