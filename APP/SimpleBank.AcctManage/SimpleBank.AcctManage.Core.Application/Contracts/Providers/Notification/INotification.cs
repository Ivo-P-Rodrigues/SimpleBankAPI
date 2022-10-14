namespace SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification
{
    public interface INotification
    {
        string Description { get; set; }
        string ReceiverUsername { get; set; }
        string ReceiverAddress { get; set; }
    }
}