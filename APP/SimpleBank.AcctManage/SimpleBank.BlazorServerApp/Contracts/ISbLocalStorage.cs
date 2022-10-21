using System.Reflection;

namespace SimpleBank.BlazorServerApp.Contracts
{
    public interface UserStorage
    {
        Task DeleteAsync(object obj);
        Task<string?> GetAsync(string key);
        Task SetAsync(object obj);
    }
}