using SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification;

namespace SimpleBank.AcctManage.Core.Application.Models.Notification
{
    public class MailNotification : INotification
    {
        public MailNotification(string receiverUsername, string description, string receiverAddress) 
        {
            ReceiverUsername = receiverUsername;
            Description = description;
            ReceiverAddress = receiverAddress;
        }

        public string ReceiverUsername { get; set; }
        public string ReceiverAddress { get; set; }
        public string Description { get; set; }




    }
}
