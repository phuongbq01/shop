using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Models
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var UserSession = (UserLogin)HttpContext.Current.Session[Common.SessionCommonConstants.USER_SESSION];   // Lấy User trong Session
            if (UserSession == null)
                return false;
            List<string> privilegeLevels = this.GetCredentialByLoggedInUser(UserSession.UserName);  // Lấy danh sách các quyền của User

            if (privilegeLevels.Contains(this.RoleID) || UserSession.GroupID == Common.UserGroupCommonConstains.ADMIN_GROUP)  // Kiểm tra xem User có quyền không
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Nếu xác nhận User không có quyền sẽ chuyển về trang 401
            filterContext.Result = new ViewResult {
                ViewName = "~/Views/Shared/401.cshtml"
            };
        }

        private List<string> GetCredentialByLoggedInUser(string username)   // Lấy ra danh sách các quyền của User trong Session
        {
            var credentials = (List<string>)HttpContext.Current.Session[Common.SessionCommonConstants.CREDENTIALS_SESSION];
            return credentials;
        }
    }
}