using System.ComponentModel.DataAnnotations;

namespace SimpleBankAPI.Contracts
{
    public class TransferRequest
    {
        [Required, Range(1, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int FromAccountId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int ToAccountId { get; set; }


    }
}
