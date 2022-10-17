using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SimpleBank.AcctManage.API.Providers
{
    public interface IAuthenthicationProvider
    {
        Task<ActionResult?> ValidateAuthorizationAsync(IEnumerable<Claim> claims);
        Task<ActionResult?> ValidateAuthorizationOnLoginAsync(Guid userId);
        Task<ActionResult?> ValidateAuthorizationOnLogoutAsync(IEnumerable<Claim> claims);
        Task<ActionResult?> ValidateAuthorizationOnRefreshAsync(IEnumerable<Claim> claims);
    }
}