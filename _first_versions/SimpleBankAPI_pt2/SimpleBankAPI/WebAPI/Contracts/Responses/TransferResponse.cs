using SimpleBankAPI.Core.Models;
using System.Text.Json.Serialization;

namespace SimpleBankAPI.Contracts
{
    public partial class TransferResponse 
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }


        [JsonIgnore]
        public virtual AccountResponse FromAccount { get; set; } = null!;
        [JsonIgnore]
        public virtual AccountResponse ToAccount { get; set; } = null!;




    }
}