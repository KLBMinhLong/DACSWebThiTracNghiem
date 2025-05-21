using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


namespace ThiTracNghiem.Helpers
{
    public static class EmailHelper
    {
        public static async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // Lấy thông tin từ biến môi trường
            var email = Environment.GetEnvironmentVariable("SMTP_EMAIL");
            var password = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new Exception("Chưa cấu hình biến môi trường SMTP_EMAIL hoặc SMTP_PASSWORD");

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(email, password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(email, "Smart test ThiTracNghiem"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
