using SimpleBank.AcctManage.Core.Domain.Common;

namespace SimpleBank.AcctManage.Core.Domain
{
    public class User : BaseEntity
    {
        public User()
        {
            Accounts = new HashSet<Account>();
            Sessions = new HashSet<Session>();
        }


        public string Email { get; set; }
        public string Fullname { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public string Username { get; set; }

        public DateTime? PasswordChangedAt { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
