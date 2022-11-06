namespace SimpleBank.AcctManage.API.DTModels.v2.Responses
{
    public class ResponseAccount
    {
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Currency { get; set; }


    }



}
