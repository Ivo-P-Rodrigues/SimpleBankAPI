using SimpleBank.AcctManage.Core.Domain.Common;

namespace SimpleBank.AcctManage.Core.Domain
{
    public class AccountDoc : BaseEntity
    {
        public AccountDoc() {}

        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public string DocType { get; set; }
        public byte[] Content { get; set; }

        public long Size => Content.Length;
        public long SizeKb => Content.Length / 1024;
        public long SizeMb => SizeKb / 1024;


        public virtual Account Account { get; set; }

    }
}
