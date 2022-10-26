using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Persistence
{
    public interface IUserTokenRepository : ICommonRepository<UserToken>
    {
        Task<UserToken?> GetUserTokenAsync(Guid id);
    }
}