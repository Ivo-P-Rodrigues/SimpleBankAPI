using SimpleBank.AcctManage.API.DTModels.Requests;
using SimpleBank.AcctManage.API.DTModels.Responses;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.API.Profile
{
    public class EntityMapper : IEntityMapper
    {

        //account business
        public AccountResponse MapAccountModelToContract(Account modelAccount) =>
              new AccountResponse()
              {
                  AccountId = modelAccount.Id,
                  UserId = modelAccount.UserId,
                  Balance = modelAccount.Balance,
                  CreatedAt = modelAccount.CreatedAt,
                  Currency = modelAccount.Currency,
              };
        public ICollection<AccountResponse> MapAccountListModelToContract(IEnumerable<Account> modelAccounts)
        {
            List<AccountResponse> contractsAccounts = new List<AccountResponse>();
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
        public ICollection<Movim>? MapMovementEnumerableToMovim(IEnumerable<Movement>? movements)
        {
            if (movements == null) { return null; }
            List<Movim> movimsToReturn = new List<Movim>();
            movements.ToList().ForEach(movement =>
                movimsToReturn.Add(
                    MapMovementToMovim(movement)));
            return movimsToReturn;
        }
        public Account MapRequestToAccountModel(CreateAccountRequest accountRequest, Guid userId) =>
            new Account()
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
                CreatedAt = DateTime.Now
            };
        public TransferResponse MapTransferToResponse(Transfer transfer) =>
            new TransferResponse()
            {
                Amount = transfer.Amount,
                FromAccountId = transfer.FromAccountId,
                ToAccountId = transfer.ToAccountId,
            };


        //user business
        public User MapRequestToUser(CreateUserRequest createUserRequest, byte[] passwordHashed, byte[] salt) =>
            new User()
            {
                Email = createUserRequest.Email,
                Username = createUserRequest.Username,
                Password = passwordHashed,
                Salt = salt,
                Fullname = createUserRequest.Fullname,
                PasswordChangedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
            };
        public CreateUserResponse MapUserToResponse(User user) =>
            new CreateUserResponse()
            {
                UserId = user.Id,
                CreatedAt = user.CreatedAt.ToString(),
                Email = user.Email,
                Fullname = user.Fullname,
                PasswordChangedAt = user.PasswordChangedAt.ToString(),
                Username = user.Username
            };

        public LoginUserResponse MapUserTokenToLoginResponse(UserToken userToken) =>
            new LoginUserResponse()
            {
                AccessToken = userToken.AccessToken,
                AccessTokenExpiresAt = userToken.AccessTokenExpiresAt.ToString(),
                RefreshToken = userToken.RefreshToken,
                RefreshTokenExpiresAt = userToken.RefreshTokenExpiresAt.ToString(),
                UserTokenId = userToken.Id,
                UserId = userToken.UserId
            };


    }
}
