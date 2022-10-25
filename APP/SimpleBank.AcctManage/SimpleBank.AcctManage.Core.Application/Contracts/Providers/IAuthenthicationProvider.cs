using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Providers
{
    public interface IAuthenthicationProvider
    {
        Task<(UserToken?, string?)> ProcessLoginAsync(Guid userId);
        Task<(UserToken?, string?)> ProcessRenewToken(string refreshToken);
        Task<bool?> ProcessLogout(Guid userTokenId);
        Task<UserToken?> GetUserTokenAsync(Guid userId);

    }
}
