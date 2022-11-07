using System.Reflection;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v1.Contracts
{
    public interface ISbLocalStorage
    {
        Task DeleteAsync(object obj);
        Task<string?> GetAsync(string key);
        Task SetAsync(object obj);
    }
}