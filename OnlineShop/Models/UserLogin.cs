using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Dùng để Gán Session
namespace OnlineShop.Models
{
    [Serializable]
    public class UserLogin
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string GroupID { get; set; }
    }
}