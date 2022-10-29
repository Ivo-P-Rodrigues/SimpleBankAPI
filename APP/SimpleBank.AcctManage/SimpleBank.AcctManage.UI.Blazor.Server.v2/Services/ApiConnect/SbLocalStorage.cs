using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Reflection;
using System.ComponentModel;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect
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
        public async Task<object?> GetObjAsync(object obj)
        {
            if (obj == null) { throw new ArgumentNullException(nameof(obj)); }
            return await GetObjAsync(obj.GetType());
        }
        public async Task<object?> GetObjAsync(Type type)
        {
            var objProps = type.GetProperties();
            var objToReturn = Activator.CreateInstance(type);
            TypeConverter typeConverter;
            object? propValue;

            foreach (PropertyInfo prop in objProps)
            {
                var storagedProp = await _protectedLocalStorage.GetAsync<string>(_configuration["Purpose"], prop.Name);
                if (storagedProp.Success)
                {
                    typeConverter = TypeDescriptor.GetConverter(prop.PropertyType);
                    try { propValue = typeConverter.ConvertFromString(storagedProp.Value!); }
                    catch { return null; }
                    prop.SetValue(objToReturn, propValue);
                }
                else
                {
                    return null; //all props must be filled, else breaks
                }
            }
            return objToReturn;
        }

        //SET
        public async Task SetObjAsync(object obj)
        {
            if (obj == null) { throw new ArgumentNullException(nameof(obj)); }

            PropertyInfo[] objProps = obj.GetType().GetProperties();

            foreach (PropertyInfo prop in objProps)
            {
                await _protectedLocalStorage.SetAsync(_configuration["Purpose"], prop.Name, prop.GetValue(obj)!.ToString()!);
            }
        }

        //DELETE
        public async Task DeleteAsync(string key) =>
            await _protectedLocalStorage.DeleteAsync(key);
        public async Task DeleteObjAsync(object obj)
        {
            if (obj == null) { throw new ArgumentNullException(nameof(obj)); }

            PropertyInfo[] objProps = obj.GetType().GetProperties();

            foreach (PropertyInfo prop in objProps)
            {
                await _protectedLocalStorage.DeleteAsync(prop.Name);
            }
        }





    }
}


