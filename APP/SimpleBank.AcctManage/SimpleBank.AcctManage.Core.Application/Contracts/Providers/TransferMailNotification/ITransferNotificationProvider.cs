using SimpleBank.AcctManage.Core.Application.Models;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification
{
    public interface ITransferNotificationProvider
    {
        void SendNotification(TransferMailNotification notification);

        TransferMailNotification DeserializeNotice(string messageValue);

    }
}
