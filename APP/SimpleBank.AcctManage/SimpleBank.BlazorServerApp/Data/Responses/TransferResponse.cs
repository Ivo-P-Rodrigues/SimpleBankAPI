namespace SimpleBank.BlazorServerApp.Data.Responses
{
    public class TransferResponse
    {
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public decimal Amount { get; set; }


    }
}
