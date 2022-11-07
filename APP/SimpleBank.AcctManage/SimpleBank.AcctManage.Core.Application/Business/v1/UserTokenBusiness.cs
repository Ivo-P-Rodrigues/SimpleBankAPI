using SimpleBank.AcctManage.Core.Application.Contracts.Business.v1;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Core.Domain;
using System.Linq.Expressions;

namespace SimpleBank.AcctManage.Core.Application.Business.v1
{
    /// <summary>
    /// Basically works as a connector between UnitOfWork and AuthenticationProvider. <para />
    /// UnitOfWork -> UserTokenBusiness -> AuthenticationProvider <para />
    /// This way, two Infrastructure members don't have an direct access between each other (could just inject UnitOfWork in Auth Prov)
    /// </summary>
    public class UserTokenBusiness : IUserTokenBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserTokenBusiness(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<UserToken?> GetUserTokenAsync(Expression<Func<UserToken, bool>> predicate) =>
            await _unitOfWork.UserTokens.GetWhereAsync(predicate);

        public async Task<UserToken?> DirectUpdateAsync(UserToken userToken) =>
            await _unitOfWork.UserTokens.DirectUpdateAsync(userToken);

        public async Task<UserToken?> DirectAddAsync(UserToken userToken) =>
            await _unitOfWork.UserTokens.DirectAddAsync(userToken);





    }
}
