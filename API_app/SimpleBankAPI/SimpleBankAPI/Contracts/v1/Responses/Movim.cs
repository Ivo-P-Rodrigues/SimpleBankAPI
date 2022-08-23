using System.ComponentModel.DataAnnotations;

namespace SimpleBankAPI.Contracts
{
    public class Movim
    {
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
