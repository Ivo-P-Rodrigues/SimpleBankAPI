using SimpleBank.AcctManage.Core.Domain;
using System.Linq.Expressions;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Business.v2
{
    public interface IUserTokenBusiness
    {
        Task<UserToken?> DirectAddAsync(UserToken userToken);
        Task<UserToken?> DirectUpdateAsync(UserToken userToken);
        Task<UserToken?> GetUserTokenAsync(Expression<Func<UserToken, bool>> predicate);
    }
}