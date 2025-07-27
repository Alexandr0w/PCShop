using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using PCShop.Web.Infrastructure.Settings;
using System.Net;
using System.Net.Mail;

namespace PCShop.Web.Infrastructure.Emailing
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;

        public EmailSender(IOptions<EmailSettings> options)
        {
            this._settings = options.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(this._settings.Username, this._settings.SenderName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            mail.To.Add(toEmail);

            using SmtpClient client = new SmtpClient(this._settings.SmtpServer, this._settings.SmtpPort)
            {
                Credentials = new NetworkCredential(this._settings.Username, this._settings.Password),
                EnableSsl = true
            };

            await client.SendMailAsync(mail);
        }
    }
}