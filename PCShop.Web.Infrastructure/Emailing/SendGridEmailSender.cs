using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PCShop.Web.Infrastructure.Emailing
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public SendGridEmailSender(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string? apiKey = this._configuration["SendGrid:ApiKey"];
            string? fromEmail = this._configuration["SendGrid:FromEmail"];
            string? fromName = this._configuration["SendGrid:FromName"] ?? "PCShop";

            SendGridClient client = new SendGridClient(apiKey);
            EmailAddress from = new EmailAddress(fromEmail, fromName);
            EmailAddress to = new EmailAddress(email);
            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, StripHtmlTags(htmlMessage), htmlMessage);

            await client.SendEmailAsync(msg);
        }

        private static string StripHtmlTags(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "<.*?>", string.Empty);
        }
    }
}