using System.ComponentModel.DataAnnotations;

namespace SimpleBank.BlazorServerApp.Data.Requests
{
    public class TransferRequest
    {
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }

    }
}
