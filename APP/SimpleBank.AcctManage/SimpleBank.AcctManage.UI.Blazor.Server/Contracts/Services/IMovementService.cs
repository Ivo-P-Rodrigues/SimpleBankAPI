using SimpleBank.AcctManage.UI.Blazor.Server.Data;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Contracts.Services
{
    public interface IMovementService
    {
        Task<IEnumerable<Movement>?> GetAll(Guid accountId);
    }
}