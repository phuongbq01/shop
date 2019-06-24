using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class EmailHelper
    {
        // Gửi Email
        public void SendEmail(string ToEmailAddress, string subject, string content)
        {
            // Lấy thông tin từ Appsetting
            var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
            var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
            var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
            bool enabledSSL = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());

            string body = content;      // Nội dung Email
            // Tạo mới 1 email với 2 tham số truyền vào là địa chỉ email gửi và địa chỉ email nhận
            MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(ToEmailAddress));
            message.Subject = subject;  // Tiêu đề email
            message.IsBodyHtml = true;  // cho phép gen từ html thành nội dung email
            message.Body = body;        // Nội dung Email

            // Tạo 1 SmtpClient để gửi Email
            var client = new SmtpClient();
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);    // Truyền vào tên đăng nhập và mật khẩu của email gửi
            client.Host = smtpHost;
            client.EnableSsl = enabledSSL;
            client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            client.Send(message);       // gửi email
        }
    }
}
