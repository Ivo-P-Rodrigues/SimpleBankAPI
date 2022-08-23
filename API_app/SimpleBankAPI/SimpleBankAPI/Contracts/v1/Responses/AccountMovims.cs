
namespace SimpleBankAPI.Contracts
{
    public class AccountMovims
    {
        public AccountMovims(Contracts.AccountResponse account, ICollection<Movim> movims)
        {
            Account = account;
            Movims = movims;
        }

        public Contracts.AccountResponse Account { get; set; }

        public virtual ICollection<Movim> Movims { get; set; }


    }
}
