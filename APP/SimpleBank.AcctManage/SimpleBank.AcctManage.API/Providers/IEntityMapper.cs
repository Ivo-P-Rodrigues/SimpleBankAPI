﻿using SimpleBank.AcctManage.API.DTModels.Requests;
using SimpleBank.AcctManage.API.DTModels.Responses;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.API.Providers
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
        LoginUserResponse MapSessionToLoginResponse(Session session);
        TransferResponse MapTransferToResponse(Transfer transfer);
        CreateUserResponse MapUserToResponse(User user);


    }
}