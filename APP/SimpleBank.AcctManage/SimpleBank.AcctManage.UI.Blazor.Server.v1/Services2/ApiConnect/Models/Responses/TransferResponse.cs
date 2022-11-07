namespace SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Models.Responses
{
    public class TransferResponse
    {
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public decimal Amount { get; set; }


    }
}
