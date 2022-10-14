using Serilog;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Text;
using SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification;
using SimpleBank.AcctManage.Core.Application.Models.Notification;

namespace SimpleBank.AcctManage.Infrastructure.Providers.NotificationManager.Providers
{
    public class NotificationEmailProvider<T> : INotificationProvider<MailNotification>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public NotificationEmailProvider(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger;
        }

        public void SendNotification(MailNotification notification)
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

        public MailNotification DeserializeNotice(string messageValue) =>
            JsonSerializer.Deserialize<MailNotification>(messageValue)!;


        private MailMessage MakeMailMessage(MailNotification notification) =>
            new MailMessage(
                from: _configuration["MailService:SystemAddress"],
                to: notification.ReceiverAddress,
                subject: "You have a transfer notification.",
                body: MakeMailBody(notification))
            { IsBodyHtml = true };


        private string MakeMailBody(MailNotification notification)
        {
            string blue = "color:blue;";
            string green = "color:green;";
            string center = "text-align:center;";

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"<h3 style=\"{blue + center}\">SimpleBank</h3>");
            stringBuilder.AppendLine($"<h4 style=\"{blue + center}\">You just received a transfer!</h4>");
            stringBuilder.AppendLine($"<hr /><br /><br />");
            stringBuilder.AppendLine($"<p style=\"{green + center}\">{notification.Description}.</p>");
            stringBuilder.AppendLine($"<p style=\"{green + center}\">Have a nice day!</p>");
            stringBuilder.AppendLine($"<br /><br /><br /><br /><br /><br /><hr />");
            stringBuilder.AppendLine($"<p>This is a test.</p>");
            return stringBuilder.ToString();
        }


    }
}
