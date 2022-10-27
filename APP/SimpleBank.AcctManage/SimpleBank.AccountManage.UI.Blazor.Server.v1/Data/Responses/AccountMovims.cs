namespace SimpleBank.AccountManage.UI.Blazor.Server.v1.Data.Responses
{
    public class AccountMovims
    {
        public AccountMovims(AccountResponse account, ICollection<Movim>? movims)
        {
            Account = account;
            Movims = movims ?? null;
        }

        public AccountResponse Account { get; set; }
        public virtual ICollection<Movim>? Movims { get; set; }


    }
}
