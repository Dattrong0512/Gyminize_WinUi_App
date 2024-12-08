using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
namespace Gyminize.Services;


/// <summary>
/// Lớp này chịu trách nhiệm gửi email thông qua dịch vụ SMTP.
/// </summary>
public class EmailSender : IEmailSender
{
    /// <summary>
    /// Gửi email bất đồng bộ đến địa chỉ email chỉ định với tiêu đề và nội dung.
    /// </summary>
    /// <param name="email">Địa chỉ email người nhận.</param>
    /// <param name="subject">Tiêu đề của email.</param>
    /// <param name="message">Nội dung của email, có thể là HTML.</param>
    /// <returns>Trả về một tác vụ bất đồng bộ.</returns>
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587, // TLS
                Credentials = new NetworkCredential("gyminizeapp@gmail.com", "qvrt ygwq ovse hkok"),
                EnableSsl = true, // Bật kết nối bảo mật
                
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("gyminizeapp@gmail.com"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true 
            };

            mailMessage.To.Add(email); 

            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }
}

