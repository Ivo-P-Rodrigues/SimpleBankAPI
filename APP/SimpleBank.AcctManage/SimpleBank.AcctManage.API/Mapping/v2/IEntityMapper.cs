using SimpleBank.AcctManage.API.DTModels.v2.Requests;
using SimpleBank.AcctManage.API.DTModels.v2.Responses;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.API.Mapping.v2
{
    public interface IEntityMapper
    {

        ResponseAuthLogin Map(UserToken userToken);
        Transfer Map(RequestTransferCreate transferRequest);

    
        ResponseAccount Map(Account modelAccount);
        IEnumerable<ResponseAccount> Map(IEnumerable<Account> modelAccounts);
        Account Map(RequestAccountCreate accountRequest, Guid userId);


        ResponseMovement Map(Movement movement);
        IEnumerable<ResponseMovement> Map(IEnumerable<Movement> movements);


        ResponseAccountDoc Map(AccountDoc accountDoc);
        IEnumerable<ResponseAccountDoc> Map(IEnumerable<AccountDoc> accountDocs);

    }
}