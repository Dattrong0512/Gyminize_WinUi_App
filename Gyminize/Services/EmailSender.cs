using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
namespace Gyminize.Services;



public class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            // Cấu hình SMTP Client
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587, // TLS
                Credentials = new NetworkCredential("gyminizeapp@gmail.com", "qvrt ygwq ovse hkok"),
                EnableSsl = true, // Bật kết nối bảo mật
                
            };

            // Cấu hình MailMessage
            var mailMessage = new MailMessage
            {
                From = new MailAddress("gyminizeapp@gmail.com"), // Email gửi
                Subject = subject,
                Body = message,
                IsBodyHtml = true // Nếu nội dung email là HTML
            };

            mailMessage.To.Add(email); // Email nhận

            // Gửi email
            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            // Xử lý lỗi
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }
}

