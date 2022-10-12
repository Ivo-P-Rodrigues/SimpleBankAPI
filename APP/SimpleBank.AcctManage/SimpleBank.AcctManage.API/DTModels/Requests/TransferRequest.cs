namespace SimpleBank.AcctManage.API.DTModels.Requests
{
    public class TransferRequest
    {
        public decimal Amount { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }

    }
}
