using SimpleBank.AcctManage.Core.Application.Contracts.Business.v2;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Core.Domain;
using System.Linq.Expressions;

namespace SimpleBank.AcctManage.Core.Application.Business.v2
{
    /// <summary> UnitOfWork -> UserTokenBusiness -> AuthenticationProvider </summary>
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
