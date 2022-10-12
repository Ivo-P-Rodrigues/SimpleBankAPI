using SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification;
using SimpleBank.AcctManage.Core.Application.NotificationModels;
using System.Net;
using System.Net.Mail;
using System.Text.Json;

namespace SimpleBank.AcctManage.Infrastructure.Providers.Notification.Providers
{
    public class EmailProvider : INotificationProvider<TransferMailNotice>
    {
        private readonly string _systemMail = "wsMakerSynergy@gmail.com";
        private readonly string _systemPass = "lssfslxfxuuuzsen";
        private readonly string _mailBody = "<h3>This is my first Templated Email in C#</h3>";


        public void SendNotification(TransferMailNotice notification)
        {
            MailMessage newMail = new MailMessage(
                from: _systemMail,
                to: notification.ToEmail,
                subject: "You have a transfer notification.",
                body: _mailBody + notification.Amount)
            { IsBodyHtml = true };

            try
            {
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)) //465
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_systemMail, _systemPass);
                    smtp.EnableSsl = true;
                    smtp.Send(newMail);
                    Console.WriteLine("Email Sent");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error -" + ex);
            }
        }

        public TransferMailNotice DeserializeNotice(string messageValue) =>
            JsonSerializer.Deserialize<TransferMailNotice>(messageValue)!;

        /*
        public void bla()
        {
            var secretsId = Assembly.GetExecutingAssembly().GetCustomAttribute<UserSecretsIdAttribute>().UserSecretsId;
            var secretsPath = Microsoft.Extensions.Configuration.UserSecrets.PathHelper.GetSecretsPathFromSecretsId(secretsId);
            var secretsJson = File.ReadAllText(secretsPath);
            dynamic secrets = JsonConvert.DeserializeObject<ExpandoObject>(secretsJson, new ExpandoObjectConverter());
        }
        */
    }
}
