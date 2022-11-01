using System.ComponentModel.DataAnnotations;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests
{
    public class CreateAccountRequest
    {
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be 3 characters long.")]
        public string Currency { get; set; }

    }
}
