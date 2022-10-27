namespace SimpleBank.AccountManage.UI.Blazor.Server.v1.Data.Responses
{
    public class TransferResponse
    {
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public decimal Amount { get; set; }


    }
}
