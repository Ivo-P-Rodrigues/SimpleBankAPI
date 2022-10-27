namespace SimpleBank.AcctManage.API.DTModels.v1.Responses
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
