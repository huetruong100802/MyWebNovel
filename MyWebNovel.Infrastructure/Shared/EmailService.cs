using MyWebNovel.Application.Shared;
using MyWebNovel.Infrastructure.Settings;
using System.Net;
using System.Net.Mail;

namespace MyWebNovel.Infrastructure.Shared
{
    public class EmailService(Smtp smtpSettings) : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(host: smtpSettings.Host, port: smtpSettings.Port)
            {
                Credentials = new NetworkCredential(userName: smtpSettings.Username, password: smtpSettings.Password),
                EnableSsl = smtpSettings.UseSsl
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings.FromAddress, smtpSettings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                To = { to }
            };

            try
            {
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while sending the email: {ex.Message}");
            }
        }
    }
}
