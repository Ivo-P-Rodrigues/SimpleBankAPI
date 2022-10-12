using SimpleBank.AcctManage.Core.Domain.Common;

namespace SimpleBank.AcctManage.Core.Domain
{
    public class Movement : BaseEntity
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }

        public virtual Account Account { get; set; }

    }
}
