namespace SimpleBank.AcctManage.Core.Application.Models
{
    public class TransferMailNotification
    {
        public TransferMailNotification(string fromUser, string toUser, string toAccountId, string toEmail, string amount)
        {
            FromUser = fromUser;
            ToUser = toUser;
            ToAccountId = toAccountId;
            ToEmail = toEmail;
            Amount = amount;
        }

        //public Guid Id { get; set; }
        //public DateTime CreatedAt { get; set; } = DateTime.Now;
		
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string ToAccountId { get; set; }
        public string ToEmail { get; set; }
        public string Amount { get; set; }



    }
}
