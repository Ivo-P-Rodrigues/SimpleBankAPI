using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Business
{
    public interface IUserBusiness
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        Task<User?> CreateUser(User user, string password);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        bool VerifyUserCredentials(string password, string username, out Guid userId);
    }
}