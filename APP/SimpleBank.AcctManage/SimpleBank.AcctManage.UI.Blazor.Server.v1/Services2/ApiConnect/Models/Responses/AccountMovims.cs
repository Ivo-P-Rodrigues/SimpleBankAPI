namespace SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Models.Responses
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
