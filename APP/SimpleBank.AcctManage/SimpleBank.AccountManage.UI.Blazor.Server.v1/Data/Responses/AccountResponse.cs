namespace SimpleBank.AccountManage.UI.Blazor.Server.v1.Data.Responses
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
