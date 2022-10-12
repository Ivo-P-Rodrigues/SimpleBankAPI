
namespace SimpleBank.AcctManage.API.DTModels.Responses
{
    public class TransferResponse
    {
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public decimal Amount { get; set; }


    }
}
