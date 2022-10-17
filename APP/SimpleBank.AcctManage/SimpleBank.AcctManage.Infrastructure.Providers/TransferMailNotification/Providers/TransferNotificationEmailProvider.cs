using Serilog;
using SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using SimpleBank.AcctManage.Core.Application.Models;
using System.Text;

namespace SimpleBank.AcctManage.Infrastructure.Providers.Notification.Providers
{
    public class TransferNotificationEmailProvider : ITransferNotificationProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public TransferNotificationEmailProvider(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger;
        }

        public void SendNotification(TransferMailNotification notification)
        {
            var newMail = MakeMailMessage(notification);

            try
            {
                using (SmtpClient smtp = new SmtpClient(
                    _configuration["MailService:SmtpClient"],
                    int.Parse(_configuration["MailService:SmtpClientPort"]))) //465
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

        public TransferMailNotification DeserializeNotice(string messageValue) =>
            JsonSerializer.Deserialize<TransferMailNotification>(messageValue)!;

        
        private MailMessage MakeMailMessage(TransferMailNotification notification) =>
            new MailMessage(
                from: _configuration["MailService:SystemAddress"],
                to: notification.ToEmail,
                subject: "You have a transfer notification.",
                body: MakeMailBody(notification))
                { IsBodyHtml = true };
       
        private string MakeMailBody(TransferMailNotification notification)
        {
            string blue = "color:blue;";
            string green = "color:green;";
            string center = "text-align:center;";

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"<h3 style=\"{blue + center}\">SimpleBank</h3>");
            stringBuilder.AppendLine($"<h4 style=\"{blue + center}\">You just received a transfer!</h4>");
            stringBuilder.AppendLine($"<hr /><br /><br />");
            stringBuilder.AppendLine($"<p style=\"{green + center}\">You received {notification.Amount} on your account {notification.ToAccountId} from {notification.FromUser}.</p>");
            stringBuilder.AppendLine($"<p style=\"{green + center}\">Have a nice day!</p>");
            stringBuilder.AppendLine($"<br /><br /><br /><br /><br /><br /><hr />");
            stringBuilder.AppendLine($"<p>This is a test.</p>");
            return stringBuilder.ToString();
        }
        

    }
}
