using SimpleBankAPI.Contracts;
using SimpleBankAPI.Core.Models;

namespace SimpleBankAPI.WebAPI
{
    public class EntityMapper : IEntityMapper
    {

        //account business
        public AccountResponse MapAccountModelToContract(Account modelAccount) =>
              new AccountResponse()
              {
                  AccountId = modelAccount.AccountId,
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
            if(movements == null) { return null; }
            List<Movim> movimsToReturn = new List<Movim>();
            movements.ToList().ForEach(movement =>
                movimsToReturn.Add(
                    MapMovementToMovim(movement)));
            return movimsToReturn;
        }
        public Account MapRequestToAccountModel(CreateAccountRequest accountRequest, int userId) =>
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
                UserId = user.UserId,
                CreatedAt = user.CreatedAt.ToString(),
                Email = user.Email,
                Fullname = user.Fullname,
                PasswordChangedAt = user.PasswordChangedAt.ToString(),
                Username = user.Username
            };

        public LoginUserResponse MapSessionToLoginResponse(Session session) =>
            new LoginUserResponse()
            {
                AccessToken = session.AccessToken,
                AccessTokenExpiresAt = session.AccessTokenExpiresAt.ToString(),
                RefreshToken = session.RefreshToken,
                RefreshTokenExpiresAt = session.RefreshTokenExpiresAt.ToString(),
                SessionId = session.SessionId,
                UserId = session.UserId
            };

    }
}
