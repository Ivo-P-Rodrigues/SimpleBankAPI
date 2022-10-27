namespace SimpleBank.AcctManage.API.DTModels.v1.Responses
{
    public class TransferResponse
    {
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public decimal Amount { get; set; }


    }
}
