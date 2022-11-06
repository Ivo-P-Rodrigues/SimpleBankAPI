namespace SimpleBank.AcctManage.API.DTModels.v1.Responses
{
    public class AccountDetailsResponse
    {
        public AccountDetailsResponse(
            AccountResponse account,
            ICollection<Movim>? movims,
            ICollection<AccountDocResponse>? docs)
        {
            Account = account;
            Movims = movims;
            Docs = docs;
        }

        public AccountResponse Account { get; set; }
        public virtual ICollection<Movim>? Movims { get; set; }
        public virtual ICollection<AccountDocResponse>? Docs { get; set; }


    }
}
