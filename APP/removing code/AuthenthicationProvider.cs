using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.API.Providers
{
    //Note: class using Microsoft.AspNetCore.Mvc to be able to return ActionResults.
    //      in principle, this should be reserved for the Controllers

    public class AuthenthicationProvider : IAuthenthicationProvider
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthenthicationProvider(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }


        public async Task<ActionResult?> ValidateAuthorizationAsync(IEnumerable<Claim> claims)
        {
            if (!UserClaimsValidation(claims, out Session session))
            {
                return new UnauthorizedObjectResult("Invalid access."); //invalid claims in token
            }

            var (toClose, refresh, access) = await ValidateSessionTimes(session);

            if (toClose) //only closes non-refreshables
            {
                session.Active = false;
                var checkClosure = await _unitOfWork.Sessions.DirectUpdateAsync(session);
                //if (checkClosure == null) { return new UnprocessableEntityObjectResult("Error on closing pre-existing session."); }
                if (checkClosure == null) { return new ObjectResult("Error on closing pre-existing expired session.") { StatusCode = 500 }; }
            }

            if (refresh) { return new BadRequestObjectResult("Please refresh your connection."); }
            if (!refresh && !access) { return new BadRequestObjectResult("Please login first."); }

            return null;
        }
        public async Task<ActionResult?> ValidateAuthorizationOnRefreshAsync(IEnumerable<Claim> claims)
        {
            if (!UserClaimsValidation(claims, out Session? session))
            {
                return new UnauthorizedObjectResult("Invalid access."); //invalid claims in token
            }

            var (toClose, refresh, access) = await ValidateSessionTimes(session!);

            if (toClose)
            {
                session!.Active = false;
                var checkClosure = await _unitOfWork.Sessions.DirectUpdateAsync(session);
                if (checkClosure == null) { return new ObjectResult("Error on closing pre-existing session.") { StatusCode = 500 }; }
            }

            if (!refresh && !access) { return new BadRequestObjectResult("Refresh no longer possible, please login."); }
            if (access) { return new BadRequestObjectResult("Access still open, no need to refresh."); }

            return null;
        }
        public async Task<ActionResult?> ValidateAuthorizationOnLogoutAsync(IEnumerable<Claim> claims)
        {
            if (!UserClaimsValidation(claims, out Session? session))
            {
                return new UnauthorizedObjectResult("Invalid access."); //invalid claims in token
            }

            var (_, _, access) = await ValidateSessionTimes(session!);

            if (!access) { return new BadRequestObjectResult("Logout unavailable."); }

            return null;
        }
        public async Task<ActionResult?> ValidateAuthorizationOnLoginAsync(Guid userId)
        {
            var userSession = _unitOfWork.Sessions.GetLastSessionOfUser(userId);
            if (userSession == null) { return null; } //on first time login

            var (toClose, refresh, access) = await ValidateSessionTimes(userSession);

            if (toClose) //only closes non-refreshables
            {
                userSession.Active = false;
                var checkClosure = await _unitOfWork.Sessions.DirectUpdateAsync(userSession);
                if (checkClosure == null) { return new ObjectResult("Error on closing pre-existing expired session.") { StatusCode = 500 }; }
            }

            if (access) { return new BadRequestObjectResult("Already logged in."); }
            if (refresh) { return new BadRequestObjectResult("Please refresh your connection instead of login in."); }

            return null;
        }

        private async Task<(bool toClose, bool refresh, bool access)> ValidateSessionTimes(Session session)
        {
            if (session == null || !session.Active) { return (false, false, false); }          // session closed or null
            if (session.Active &&
                session.RefreshTokenExpiresAt < DateTime.Now) { return (true, false, false); } // session open but refresh ended -> needs closure
            if (session.AccessTokenExpiresAt < DateTime.Now &&
                session.RefreshTokenExpiresAt > DateTime.Now) { return (false, true, false); } // access ended and refresh open
            return (false, false, true);                                                       // access still open
        }
        private bool UserClaimsValidation(IEnumerable<Claim> claims, out Session? userSession)
        {
            if (!Guid.TryParse(claims.FirstOrDefault(c => c.Type == "userId")?.Value, out Guid userId) ||
            !Guid.TryParse(claims.FirstOrDefault(c => c.Type == "sessionId")?.Value, out Guid sessionId))
            { userSession = null; return false; }

            var user = _unitOfWork.Users.Get(userId);
            var session = _unitOfWork.Sessions.Get(sessionId);
            if (user == null || session == null || session.UserId != user.Id)
            { userSession = null; return false; }

            userSession = _unitOfWork.Sessions.GetLastSessionOfUser(userId);
            if (session.Id != userSession?.Id ||
                session.AccessToken != userSession.AccessToken)
            { return false; }


            return true;
        }


    }
}
