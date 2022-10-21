﻿using Microsoft.AspNetCore.Components;
using SimpleBank.BlazorServerApp.Contracts;
using SimpleBank.BlazorServerApp.Data.Requests;
using SimpleBank.BlazorServerApp.Data.Responses;
using System.Net.Http.Headers;

namespace SimpleBank.BlazorServerApp.Services.Base
{
    public abstract class BaseService
    {
        private readonly HttpClient _httpClient;
        private readonly IUserStorage _userStorage;


        public BaseService(
            HttpClient httpClient,
            IUserStorage userStorage)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _userStorage = userStorage ?? throw new ArgumentNullException(nameof(userStorage));
        }




        public async Task<HttpResponseMessage?> GetAsync(string? requestUri, bool auth = false)
        {
            if (auth)
            {
                if (!await ResolveAuthAsync()) { return null; }
            }

            return await _httpClient.GetAsync(requestUri);
        }
        public async Task<HttpResponseMessage?> PostAsync<TValue>(string? requestUri, TValue value, bool auth = false)
        {
            if(auth)
            {
                if(!await ResolveAuthAsync()) { return null; }
            }

            return await _httpClient.PostAsJsonAsync(requestUri, value);
        }





        public async Task<bool> RegisterLogin(LoginUserRequest loginUserRequest)
        {
            var httpRsp = await PostAsync("/api/users/login", loginUserRequest);
            if (httpRsp == null) { return false; }

            if (httpRsp.IsSuccessStatusCode)
            {
                var response = await httpRsp.Content.ReadFromJsonAsync(typeof(LoginUserResponse)) as LoginUserResponse;
                if (response == null) { return false; }

                await _userStorage.SetUserInfo(response);
                return true;
            }
            return false;
        }
        public async Task<bool> RegisterLogout()
        {
            if (!await ResolveAuthAsync()) { return false; }

            var storageTokenId = await _userStorage.GetUserTokenId();
            if (storageTokenId == null) { return false; }

            var logoutUserRequest = new LogoutUserRequest() { UserTokenId = Guid.Parse(storageTokenId) };
            await _userStorage.DeleteUserInfo();

            var httpRsp = await _httpClient.PostAsJsonAsync("logout", logoutUserRequest);
            if (httpRsp.IsSuccessStatusCode)
            {
                await _userStorage.DeleteUserInfo();
                //return await httpRsp.Content.ReadAsStringAsync();
                return true;
            }

            return true;
        }


        #region Auth handeling
        private async Task<bool> ResolveAuthAsync()
        {
            var (accVal, rfhVal) = await CheckConnectionAsync();

            if (!accVal && !rfhVal) { return false; }                   //storage empty or access not valid  

            if (accVal && rfhVal)                                       //access valid, add authorization to request
            {
                if (await AddAuthorizationAsync()) { return true; }     //try to add authorization to header
            }

            if (!accVal && rfhVal)                                      //access invalid but refresh still valid 
            {
                if (await RefreshTheConnectionAsync()) { return true; } // try do a refresh
                if (await AddAuthorizationAsync()) { return true; }     // try to add authorization to header 
            }

            return false;
        }
        /// <summary>
        /// Checks the Access token and Refresh token validity in the local storage.
        /// </summary>
        /// <returns>
        /// (false, false) if storage is empty or both tokens invalid; <para />
        /// (true, true)   if access stil valid; <para />
        /// (false, true)  if access invalid but refresh still valid.
        /// </returns>
        private async Task<(bool, bool)> CheckConnectionAsync()
        {
            var (validAccStorage, validAccToken) = await _userStorage.CheckAccessValidity();
            if(!validAccStorage) { return (false, false); } // no access token found
            if(validAccToken) { return (true, true); }      // access token is valid

            var (validRfhStorage, validRfhToken) = await _userStorage.CheckRefreshValidity();
            if (!validRfhStorage) { return (false, false); } // no refresh token found
            if (validRfhToken) { return (false, true); }     // refresh token is valid

            return (false, false);                           // refresh and access not valid
        }
        private async Task<bool> AddAuthorizationAsync()
        {
            var storageAccToken = await _userStorage.GetAccessToken();
            if (storageAccToken != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", storageAccToken);
                return true;
            }
            return false;
        }
        private async Task<bool> RefreshTheConnectionAsync()
        {
            var refreshToken = await _userStorage.GetRefreshToken();
            if (refreshToken == null) { return false; }
            
            var renewRequest = new RenewRequest() { RefreshToken = refreshToken };
            var httpRsp = await _httpClient.PostAsJsonAsync("/api/users/renew", renewRequest);

            if (httpRsp.IsSuccessStatusCode)
            {
                var response = await httpRsp.Content.ReadFromJsonAsync(typeof(LoginUserResponse)) as LoginUserResponse;
                await _userStorage.SetUserInfo(response!);
                return true;
            }
            return false;
        }
        #endregion







        //_navManager = navManager ?? throw new ArgumentNullException(nameof(navManager));
        //private void RedirectToHome() =>
        //    _navManager.NavigateTo(RouteAddress.Home);

        //private readonly HttpRequestMessage _httpRequestMessage;
        //public SimpleBankRequest(string method, string requestUri)
        //{
        //    _httpRequestMessage = new HttpRequestMessage(
        //        method: new HttpMethod(method),
        //        requestUri: requestUri
        //    );

        }
    }