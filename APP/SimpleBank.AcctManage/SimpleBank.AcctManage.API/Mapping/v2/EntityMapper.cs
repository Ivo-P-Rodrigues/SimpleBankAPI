using SimpleBank.AcctManage.Core.Domain;
using SimpleBank.AcctManage.API.DTModels.v2.Responses;
using SimpleBank.AcctManage.API.DTModels.v2.Requests;
using System.Xml.Linq;


namespace SimpleBank.AcctManage.API.Mapping.v2
{
    public class EntityMapper : IEntityMapper
    {
        //AUTH - Domain to Response
        public ResponseAuthLogin Map(UserToken userToken) =>
            new ResponseAuthLogin()
            {
                AccessToken = userToken.AccessToken,
                RefreshToken = userToken.RefreshToken,
                UserTokenId = userToken.Id,
            };



        //TRANSFER - Request to Domain
        public Transfer Map(RequestTransferCreate transferRequest) =>
            new Transfer()
            {
                Amount = transferRequest.Amount,
                FromAccountId = transferRequest.FromAccountId,
                ToAccountId = transferRequest.ToAccountId,
                CreatedAt = DateTime.UtcNow
            };



        //ACCOUNT - Domain to Response
        public ResponseAccount Map(Account modelAccount) =>
              new ResponseAccount()
              {
                  AccountId = modelAccount.Id,
                  UserId = modelAccount.UserId,
                  Balance = modelAccount.Balance,
                  CreatedAt = modelAccount.CreatedAt,
                  Currency = modelAccount.Currency,
              };
        public IEnumerable<ResponseAccount> Map(IEnumerable<Account> accounts)
        {
            foreach (var acc in accounts)
            {
                yield return Map(acc);
            }
        }


        //ACCOUNT - Request to Domain
        public Account Map(RequestAccountCreate accountRequest, Guid userId) =>
            new Account()
            {
                Balance = accountRequest.Balance,
                Currency = accountRequest.Currency,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
            };



        //MOVEMENT - Domain to Response
        public ResponseMovement Map(Movement movement) =>
            new ResponseMovement() {
                Amount = movement.Amount,
                CreatedAt = movement.CreatedAt 
            };
      
        public IEnumerable<ResponseMovement> Map(IEnumerable<Movement> movements)
        {
            foreach (var mov in movements)
            {
                yield return Map(mov);
            }
        }



        //ACCOUNTDOC - Domain to Response
        public ResponseAccountDoc Map(AccountDoc accountDoc) =>
            new ResponseAccountDoc()
            {
                AccountDocId = accountDoc.Id,
                CreatedAt = accountDoc.CreatedAt,
                Name = accountDoc.Name,
                DocType = accountDoc.DocType
            };
        public IEnumerable<ResponseAccountDoc> Map(IEnumerable<AccountDoc> accountDocs)
        {
            foreach (var acctDoc in accountDocs)
            {
                yield return Map(acctDoc);
            }
        }



    }

}
