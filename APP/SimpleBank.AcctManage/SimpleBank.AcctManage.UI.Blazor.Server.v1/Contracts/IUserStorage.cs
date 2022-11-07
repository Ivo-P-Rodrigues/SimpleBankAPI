using SimpleBank.AcctManage.UI.Blazor.Server.v1.Data.Responses;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v1.Contracts
{
    public interface IUserStorage
    {
        Task<(bool, bool)> CheckAccessValidity();
        Task<(bool, bool)> CheckRefreshValidity();
        Task DeleteUserInfo();
        Task<string?> GetAccessToken();
        Task<string?> GetRefreshToken();
        Task<string?> GetUserId();
        Task<string?> GetUserTokenId();
        Task SetUserInfo(LoginUserResponse loginUserResponse);
    }
}