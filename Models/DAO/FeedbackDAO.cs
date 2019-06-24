using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class FeedbackDAO
    {
        private OnlineShopDbContext db = null;

        public FeedbackDAO()
        {
            db = new OnlineShopDbContext();
        }

        public int Insert(Feedback fb)
        {
            try
            {
                db.Feedbacks.Add(fb);
                db.SaveChanges();
                return fb.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
