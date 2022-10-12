using SimpleBank.AcctManage.Core.Domain.Common;

namespace SimpleBank.AcctManage.Core.Domain
{
    public class Account : BaseEntity
    {
        public Account()
        {
            Movements = new HashSet<Movement>();
        }

        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; } = "EUR";

        public virtual User User { get; set; }
        public virtual ICollection<Movement> Movements { get; set; }

    }
}
