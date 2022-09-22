using SimpleBankAPI.Contracts;
using SimpleBankAPI.Core.Models;

namespace SimpleBankAPI.WebAPI
{
    public interface IEntityMapper
    {
        AccountResponse MapAccountModelToContract(Account modelAccount);
        ICollection<AccountResponse> MapAccountListModelToContract(IEnumerable<Account> modelAccounts);
        ICollection<Movim>? MapMovementEnumerableToMovim(IEnumerable<Movement>? movements);
        Movim MapMovementToMovim(Movement movement);
        Account MapRequestToAccountModel(CreateAccountRequest accountRequest, int userId);
        Transfer MapRequestToTransfer(TransferRequest transferRequest);
        User MapRequestToUser(CreateUserRequest createUserRequest, byte[] passwordHashed, byte[] salt);
        TransferResponse MapTransferToResponse(Transfer transfer);
        CreateUserResponse MapUserToResponse(User user);
        LoginUserResponse MapSessionToLoginResponse(Session session);
    }
}