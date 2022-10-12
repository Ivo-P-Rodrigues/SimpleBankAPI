using Microsoft.Extensions.Configuration.UserSecrets;
using Serilog;
using SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification;
using SimpleBank.AcctManage.Core.Application.NotificationModels;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text.Json;
using Serilog;
using Microsoft.Extensions.Configuration;

namespace SimpleBank.AcctManage.Infrastructure.Providers.Notification.Providers
{
    public class EmailProvider : INotificationProvider<TransferMailNotice>
    {
        private readonly string _mailBody = "<h3>This is my first Templated Email in C#</h3>";
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public EmailProvider(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void SendNotification(TransferMailNotice notification)
        {
            MailMessage newMail = new MailMessage(
                from: _configuration["MailService:SystemAddress"],
                to: notification.ToEmail,
                subject: "You have a transfer notification.",
                body: _mailBody + notification.Amount)
            { IsBodyHtml = true };

            try
            {
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)) //465
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_configuration["MailService:SystemAddress"], _configuration["MailService:SystemPass"]);
                    smtp.EnableSsl = true;
                    smtp.Send(newMail);
                    _logger.Information("Email Sent");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error sending mail. " + ex);
            }
        }

        public TransferMailNotice DeserializeNotice(string messageValue) =>
            JsonSerializer.Deserialize<TransferMailNotice>(messageValue)!;

        
        
    }
}
