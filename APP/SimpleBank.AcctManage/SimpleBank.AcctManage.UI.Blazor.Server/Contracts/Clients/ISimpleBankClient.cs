using Microsoft.AspNetCore.Components.Forms;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Contracts.Clients
{
    public interface ISimpleBankClient
    {
        Task<TResponse?> GetAsync<TResponse>(string requestUri, bool auth = false);
        Task<HttpContent?> GetContentAsync(string requestUri, bool auth = false);
        Task<bool> PostAsync(string requestUri, object requestObj, bool auth = false);
        Task<TResponse?> PostAsync<TResponse>(string requestUri, object requestObj, bool auth = false);
        Task<bool> PostFileAsync(string requestUri, IBrowserFile file, bool auth = false);
    }
}