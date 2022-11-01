namespace SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Models.Responses
{
    public class AccountResponse
    {
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Currency { get; set; }

    }
}
