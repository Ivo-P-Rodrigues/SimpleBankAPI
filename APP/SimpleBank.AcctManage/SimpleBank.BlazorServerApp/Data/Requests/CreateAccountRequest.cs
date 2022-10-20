namespace SimpleBank.BlazorServerApp.Data.Requests
{
    public class CreateAccountRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }

    }
}
