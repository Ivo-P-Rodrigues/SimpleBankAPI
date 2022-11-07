using System.ComponentModel.DataAnnotations;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Data
{
    public class AccountCreate
    {
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Balance { get; set; }
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be 3 characters long.")]
        public string Currency { get; set; }
    }
}
