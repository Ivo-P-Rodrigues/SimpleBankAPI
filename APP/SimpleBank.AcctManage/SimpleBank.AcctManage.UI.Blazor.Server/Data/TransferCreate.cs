using System.ComponentModel.DataAnnotations;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Data
{
    public class TransferCreate
    {
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }
    }






}
