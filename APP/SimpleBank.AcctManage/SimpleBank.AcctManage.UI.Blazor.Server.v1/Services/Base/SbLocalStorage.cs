using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SimpleBank.AcctManage.UI.Blazor.Server.v1.Contracts;
using System.Reflection;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v1.Services.Base
{
    public class SbLocalStorage : ISbLocalStorage
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly IConfiguration _configuration;


        public SbLocalStorage(
            ProtectedLocalStorage protectedLocalStorage,
            IConfiguration configuration)
        {
            _protectedLocalStorage = protectedLocalStorage ?? throw new ArgumentNullException(nameof(protectedLocalStorage));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        public async Task SetAsync(object obj)
        {
            PropertyInfo[] objProps = obj.GetType().GetProperties();

            foreach (PropertyInfo prop in objProps)
            {
                if (prop == null) { throw new ArgumentNullException(nameof(prop)); }
                await _protectedLocalStorage.SetAsync(_configuration["Purpose"], prop.Name, prop.GetValue(obj)!.ToString()!);
            }
        }
        public async Task<string?> GetAsync(string key)
        {
            var storaged = await _protectedLocalStorage.GetAsync<string>(_configuration["Purpose"], key);
            if (storaged.Success)
            {
                return storaged.Value;
            }
            return null;
        }
        public async Task DeleteAsync(object obj)
        {
            PropertyInfo[] objProps = obj.GetType().GetProperties();

            foreach (PropertyInfo prop in objProps)
            {
                await _protectedLocalStorage.DeleteAsync(prop.Name);
            }
        }







    }
}
