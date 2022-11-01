namespace SimpleBank.AcctManage.UI.Blazor.Server.Services.Responses
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
