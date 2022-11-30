using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using NuGet.Versioning;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;

namespace DevKids_v1.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public AuthMessageSenderOptions Options { get; }

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
            ILogger<EmailSender> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            // change to environment variables to work with Azure
            if (string.IsNullOrEmpty(Options.SmtpUser))
            {
                throw new Exception("Null user");
            }
            else if (string.IsNullOrEmpty(Options.SmtpPassword))
            {
                throw new Exception("Null password");
            }

            await Execute(subject, message, toEmail);
        }

        public async Task Execute(string subject, string message, string toEmail)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(Options.SmtpUser, Options.SmtpPassword);
            string sender = "do-not-reply@gmail.com";
            MailMessage msg = new MailMessage(sender, toEmail, subject, null);
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                message, null, "text/html");
            msg.AlternateViews.Add(htmlView);

            var response = client.SendMailAsync(msg);
            _logger.LogInformation(response.IsCompletedSuccessfully
                ? $"Email to {toEmail} queued successfully!"
                : $"Failure Email to {toEmail}");
        }
    }
}
