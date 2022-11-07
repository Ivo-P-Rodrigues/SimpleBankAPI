namespace SimpleBank.AcctManage.UI.Blazor.Server.v1.Data.Responses
{
    public class AccountMovims
    {
        public AccountMovims(AccountResponse account, ICollection<Movim>? movims)
        {
            Account = account;
            Movims = movims;
        }

        public AccountResponse Account { get; set; }
        public virtual ICollection<Movim>? Movims { get; set; }


    }
}
