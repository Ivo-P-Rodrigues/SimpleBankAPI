using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Reflection;

namespace SimpleBank.AccountManage.UI.Blazor.Server.v2.Services.ApiConnect
{
    public class SbLocalStorage
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

        //GET
        public async Task<string?> GetAsync(string key)
        {
            var storaged = await _protectedLocalStorage.GetAsync<string>(_configuration["Purpose"], key);
            return storaged.Success ? storaged.Value : null;
        }

        //SET
        public async Task SetObjAsync(object obj)
        {
            PropertyInfo[] objProps = obj.GetType().GetProperties();

            foreach (PropertyInfo prop in objProps)
            {
                if (prop == null) { throw new ArgumentNullException(nameof(prop)); }
                await _protectedLocalStorage.SetAsync(_configuration["Purpose"], prop.Name, prop.GetValue(obj)!.ToString()!);
            }
        }

        //DELETE
        public async Task DeleteAsync(string key) =>
            await _protectedLocalStorage.DeleteAsync(key);
        public async Task DeleteObjAsync(object obj)
        {
            PropertyInfo[] objProps = obj.GetType().GetProperties();

            foreach (PropertyInfo prop in objProps)
            {
                await _protectedLocalStorage.DeleteAsync(prop.Name);
            }
        }





    }
}
