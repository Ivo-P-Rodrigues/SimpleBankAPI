using Microsoft.AspNetCore.Components.Forms;
using SimpleBank.AcctManage.UI.Blazor.Server.Data;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Contracts.Services
{
    public interface IAccountDocService
    {
        Task<string?> Download(Guid accountId, Guid docId, string docType);
        Task<AccountDoc?> Get(Guid accountId, Guid docId);
        Task<IEnumerable<AccountDoc>?> GetAll(Guid accountId);
        Task<bool> Upload(Guid accountId, IBrowserFile file);
    }
}