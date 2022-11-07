using SimpleBank.AcctManage.UI.Blazor.Server.Data;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Responses;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Contracts.Mapper
{
    public interface IEntityMapper
    {
        RequestAccountCreate Map(AccountCreate accountCreate);
        IEnumerable<Account> Map(IEnumerable<ResponseAccount> responseAccounts);
        IEnumerable<AccountDoc> Map(IEnumerable<ResponseAccountDoc> rspAccountDocs);
        IEnumerable<Movement> Map(IEnumerable<ResponseMovement> responseMovements);
        Account Map(ResponseAccount responseAccount);
        AccountDoc Map(ResponseAccountDoc rspAccountDoc);
        Movement Map(ResponseMovement responseMovement);
        RequestTransferCreate Map(TransferCreate transferCreate);
        RequestUserCreate Map(UserCreate userCreate);
        RequestAuthLogin Map(UserLogin userLogin);
    }
}