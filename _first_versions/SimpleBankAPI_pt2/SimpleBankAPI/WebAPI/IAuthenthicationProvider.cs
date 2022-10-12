using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SimpleBankAPI.WebAPI
{
    public interface IAuthenthicationProvider
    {
        Task<ActionResult?> ValidateAuthorizationAsync(IEnumerable<Claim> claims);
        Task<ActionResult?> ValidateAuthorizationOnLogoutAsync(IEnumerable<Claim> claims);
        Task<ActionResult?> ValidateAuthorizationOnRefreshAsync(IEnumerable<Claim> claims);
        Task<ActionResult?> ValidateAuthorizationOnLoginAsync(int userId);
    }
}