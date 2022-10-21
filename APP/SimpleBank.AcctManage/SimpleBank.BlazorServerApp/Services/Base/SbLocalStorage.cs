using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SimpleBank.BlazorServerApp.Contracts;
using SimpleBank.BlazorServerApp.Data.Responses;
using System.Reflection;

namespace SimpleBank.BlazorServerApp.Services.Base
{
    public class SbLocalStorage : Contracts.UserStorage
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
                await _protectedLocalStorage.DeleteAsync(nameof(prop));
            }
        }



            



    }
}
