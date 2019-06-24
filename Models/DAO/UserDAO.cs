using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Common;

namespace Models.DAO
{
    public class UserDAO
    {
        private OnlineShopDbContext db = null;

        public UserDAO()
        {
            db = new OnlineShopDbContext();
        }

        // Thêm User
        public long Insert(User user)
        {
            if (user != null)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return user.ID;
            }
            else
                return -1;
        }


        // Thêm user đăng nhập bằng Facebook
        public long InsertForFacebook(User entity)
        {
            var user = db.Users.SingleOrDefault(x=>x.UserName == entity.UserName);
            if (user == null)   // Nếu chưa có tải khoản
            {
                db.Users.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            else    // Nếu đã có tài khoản
            {   
                return user.ID;
            }
        }

        // Hàm Update
        public bool Update(User entity)
        {
            try {
                var user = db.Users.Find(entity.ID);
                user.Name = entity.Name;
                if (!string.IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                user.Address = entity.Address;
                user.GroupID = entity.GroupID;
                user.Phone = entity.Phone;
                user.Email = entity.Email;
                user.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        // Delete
        public bool Delete(long id)
        {
            try {
                var user = db.Users.SingleOrDefault(x => x.ID == id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;

            } catch (Exception ex)
            {
                return false;
            }

        }

        // Hàm lấy ra user dựa vào ID
        public User GetByID(long id)
        {
            return db.Users.SingleOrDefault(x=>x.ID == id);
        }

        // Hàm lấy ra user dựa vào UserName
        public User GetByUserName(string username)
        {
            return db.Users.SingleOrDefault(x=>x.UserName == username);
        }


        // Thay đổi trạng thái hoạt động của User
        public bool ChangeStatus(long id)
        {
            var user = db.Users.SingleOrDefault(x => x.ID == id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }


        //
        public IEnumerable<User> ListAllPaging(string stringSearch, int page, int pageSize)
        {
            IQueryable<User> model = db.Users;
            if (!string.IsNullOrEmpty(stringSearch))
            {
                model = model.Where(x => x.UserName.Contains(stringSearch) || x.Name.Contains(stringSearch));
            }
            return model.OrderByDescending(x=>x.CreateDate).ToPagedList(page, pageSize);
        }


        // Lấy ra danh sách quyền của người dùng
        public List<string> GetListCredentials(string username)
        {
            var user = db.Users.SingleOrDefault(x=>x.UserName == username);
            var data = (from a in db.Credentials
                        join b in db.UserGroups on a.UserGroupID equals b.ID
                        join c in db.Roles on a.RoleID equals c.ID
                        where b.ID == user.GroupID
                        select new
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID
                        }).AsEnumerable().Select(x => new Credential()
                        {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID
                        });
            return data.Select(x => x.RoleID).ToList();
        }


        // Hàm Kiểm tra tài khoản đăng nhập
        public int Login(string username, string password, bool isLoginAdmin = false)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == username);

            // Tên đăng nhập không tồn tại
            if (result == null)
            {
                return 0;
            }
            else    // Tên đăng nhập có tồn tại
            {
                if (isLoginAdmin == true)   // Nếu đăng nhập với quyền admin
                {
                    if (result.GroupID == UserGroupCommonConstains.ADMIN_GROUP || result.GroupID == UserGroupCommonConstains.MOD_GROUP)     // Nếu tài khoản là ADMIN hoặc MOD
                    {
                        if (result.Status == false)     // tài khoản bị khóa
                        {
                            return -1;
                        }
                        else    //Nếu tài khoản không bị khóa
                        {
                            if (result.Password == password)    // Kiểm tra mật khẩu
                                return 1;
                            else
                                return -2;
                        }
                    }
                    else    // Tài Khoản không phải admin
                    {
                        return -3;
                    }
                }
                else    // người dùng đăng nhập
                {
                    if (result.Status == false)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.Password == password)
                            return 1;
                        else
                            return -2;
                    }
                }
            }
        }

        // Check UserName Đã Tồn Tại Chưa
        public bool CheckUserName(string username)
        {
            return db.Users.Count(x => x.UserName == username) > 0;
        }

        // Check Email Đã Tồn Tại Chưa
        public bool CheckEmail(string email)
        {
            return db.Users.Count(x=>x.Email == email) > 0;
        }
    }
}
