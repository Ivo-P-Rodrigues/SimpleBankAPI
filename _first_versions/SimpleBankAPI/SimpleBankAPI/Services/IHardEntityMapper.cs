using SimpleBankAPI.Contracts;
using SimpleBankAPI.Models;

namespace SimpleBankAPI.Services
{
    public interface IHardEntityMapper
    {
        Contracts.AccountResponse MapAccountModelToContract(Models.Account modelAccount);
        ICollection<Contracts.AccountResponse> MapAccountListModelToContract(IEnumerable<Models.Account> modelAccounts);
        ICollection<Movim> MapMovementEnumerableToMovim(IEnumerable<Movement> movements);
        Movim MapMovementToMovim(Movement movement);
        Models.Account MapRequestToAccountModel(CreateAccountRequest accountRequest, int userId);
        Transfer MapRequestToTransfer(TransferRequest transferRequest);
        User MapRequestToUser(CreateUserRequest createUserRequest);
        TransferResponse MapTransferToResponse(Transfer transfer);
        CreateUserResponse MapUserToResponse(User user);
    }
}