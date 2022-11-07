using SimpleBank.AcctManage.UI.Blazor.Server.Contracts.Mapper;
using SimpleBank.AcctManage.UI.Blazor.Server.Data;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Responses;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Services.Mapper
{
    public class EntityMapper : IEntityMapper
    {

        //AUTH - Domain to Request
        public RequestAuthLogin Map(UserLogin userLogin) =>
            new RequestAuthLogin()
            {
                Username = userLogin.Username,
                Password = userLogin.Password,
            };

        //USER - Domain to Request
        public RequestUserCreate Map(UserCreate userCreate) =>
            new RequestUserCreate()
            {
                Username = userCreate.Username,
                Email = userCreate.Email,
                Password = userCreate.Password,
                Fullname = userCreate.Fullname
            };




        //ACCOUNT - Domain to Request
        public RequestAccountCreate Map(AccountCreate accountCreate) =>
            new RequestAccountCreate()
            {
                Balance = accountCreate.Balance,
                Currency = accountCreate.Currency,
            };

        //ACCOUNT - Response to Domain
        public Account Map(ResponseAccount responseAccount) =>
            new Account()
            {
                AccountId = responseAccount.AccountId,
                Balance = responseAccount.Balance,
                Currency = responseAccount.Currency,
                UserId = responseAccount.UserId,
                CreatedAt = responseAccount.CreatedAt,
            };
        public IEnumerable<Account> Map(IEnumerable<ResponseAccount> responseAccounts)
        {
            foreach (ResponseAccount responseAccount in responseAccounts)
            {
                yield return Map(responseAccount);
            }
        }




        //TRANSFER - Domain to Request
        public RequestTransferCreate Map(TransferCreate transferCreate) =>
            new RequestTransferCreate()
            {
                FromAccountId = transferCreate.FromAccountId,
                ToAccountId = transferCreate.ToAccountId,
                Amount = transferCreate.Amount,
            };




        //MOVEMENT - Response to Domain
        public Movement Map(ResponseMovement responseMovement) =>
            new Movement()
            {
                Amount = responseMovement.Amount,
                CreatedAt = responseMovement.CreatedAt,
            };
        public IEnumerable<Movement> Map(IEnumerable<ResponseMovement> responseMovements)
        {
            foreach (ResponseMovement rspMov in responseMovements)
            {
                yield return Map(rspMov);
            }
        }




        //ACCOUNTDOC - Response to Domain
        public AccountDoc Map(ResponseAccountDoc rspAccountDoc) =>
            new AccountDoc()
            {
                AccountDocId = rspAccountDoc.AccountDocId,
                Name = rspAccountDoc.Name,
                DocType = rspAccountDoc.DocType,
                CreatedAt = rspAccountDoc.CreatedAt,
            };
        public IEnumerable<AccountDoc> Map(IEnumerable<ResponseAccountDoc> rspAccountDocs)
        {
            foreach (ResponseAccountDoc doc in rspAccountDocs)
            {
                yield return Map(doc);
            }
        }





    }

}
