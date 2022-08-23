using SimpleBankAPI.Contracts;
using SimpleBankAPI.Models;
using System.Collections.Generic;

namespace SimpleBankAPI.Services
{
    public class HardEntityMapper : IHardEntityMapper
    {

        //account business


        public Contracts.AccountResponse MapAccountModelToContract(Models.Account modelAccount) =>
              new Contracts.AccountResponse()
              {
                  AccountId = modelAccount.AccountId,
                  UserId = modelAccount.UserId,
                  Balance = modelAccount.Balance,
                  CreatedAt = modelAccount.CreatedAt,
                  Currency = modelAccount.Currency,
              };

        public ICollection<Contracts.AccountResponse> MapAccountListModelToContract(IEnumerable<Models.Account> modelAccounts)
        {
            List<Contracts.AccountResponse> contractsAccounts = new List<Contracts.AccountResponse>();
            modelAccounts.ToList().ForEach(account =>
                contractsAccounts.Add(
                    MapAccountModelToContract(account)));
            return contractsAccounts;
        }


        public Movim MapMovementToMovim(Movement movement) =>
          new Movim()
          {
              Amount = movement.Amount,
              CreatedAt = movement.CreatedAt
          };

        public ICollection<Movim> MapMovementEnumerableToMovim(IEnumerable<Movement> movements)
        {
            List<Movim> movimsToReturn = new List<Movim>();
            movements.ToList().ForEach(movement =>
                movimsToReturn.Add(
                    MapMovementToMovim(movement)));
            return movimsToReturn;
        }

        public Models.Account MapRequestToAccountModel(CreateAccountRequest accountRequest, int userId) =>
            new Models.Account()
            {
                Balance = accountRequest.Amount,
                Currency = accountRequest.Currency,
                UserId = userId,
                CreatedAt = DateTime.Now,
            };

        //transfer business
        public Transfer MapRequestToTransfer(TransferRequest transferRequest) =>
            new Transfer()
            {
                Amount = transferRequest.Amount,
                FromAccountId = transferRequest.FromAccountId,
                ToAccountId = transferRequest.ToAccountId,
                RequestDate = DateTime.Now
            };
        public TransferResponse MapTransferToResponse(Transfer transfer) =>
            new TransferResponse()
            {
                Amount = transfer.Amount,
                FromAccountId = transfer.FromAccountId,
                ToAccountId = transfer.ToAccountId,
            };


        //user business
        public User MapRequestToUser(CreateUserRequest createUserRequest) =>
            new User()
            {
                Email = createUserRequest.Email,
                Username = createUserRequest.Username,
                Password = createUserRequest.Password,
                Fullname = createUserRequest.Fullname,
                PasswordChangedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
            };

        public CreateUserResponse MapUserToResponse(User user) =>
            new CreateUserResponse()
            {
                UserId = user.UserId,
                CreatedAt = user.CreatedAt.ToString(),
                Email = user.Email,
                Fullname = user.Fullname,
                PasswordChangedAt = user.PasswordChangedAt.ToString(),
                Username = user.Username
            };



    }
}
