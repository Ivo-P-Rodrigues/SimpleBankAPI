using SimpleBank.AcctManage.API.DTModels.v1.Requests;
using SimpleBank.AcctManage.API.DTModels.v1.Responses;
using SimpleBank.AcctManage.API.DTModels.v2.Responses;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.API.Mapping.v1
{
    public interface IEntityMapper
    {
        ICollection<AccountResponse> MapAccountListModelToContract(IEnumerable<Account> modelAccounts);
        AccountResponse MapAccountModelToContract(Account modelAccount);
        ICollection<Movim>? MapMovementEnumerableToMovim(IEnumerable<Movement>? movements);
        Movim MapMovementToMovim(Movement movement);
        Account MapRequestToAccountModel(CreateAccountRequest accountRequest, Guid userId);
        Transfer MapRequestToTransfer(TransferRequest transferRequest);
        User MapRequestToUser(CreateUserRequest createUserRequest, byte[] passwordHashed, byte[] salt);
        LoginUserResponse MapToLoginResponse(UserToken uSserToken);
        TransferResponse MapTransferToResponse(Transfer transfer);
        CreateUserResponse MapUserToResponse(User user);
        ICollection<AccountDocResponse> MapAccountDocToResponse(IEnumerable<AccountDoc> accountDocs);
        AccountDocResponse MapAccountDocToResponse(AccountDoc accountDoc);


    }
}