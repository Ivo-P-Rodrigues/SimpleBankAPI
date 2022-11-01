using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect;
using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Models.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Models.Responses;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v2.Services
{
    public class AccountService
    {
        private readonly SbApiConnect _sbApiConnect;
        private readonly string _requestUri = "/api/v1/accounts/";

        public AccountService(SbApiConnect sbApiConnect)
        {
            _sbApiConnect = sbApiConnect ?? throw new ArgumentNullException(nameof(sbApiConnect));
        }
        public async Task<IEnumerable<AccountResponse>?> GetAllAccounts() =>
            await _sbApiConnect.GetAndReturnAsync<IEnumerable<AccountResponse>?>(_requestUri, true) as IEnumerable<AccountResponse>;

        public async Task<AccountMovims?> GetAccount(Guid accountId) =>
            await _sbApiConnect.GetAndReturnAsync<AccountMovims?>(_requestUri + accountId.ToString(), true) as AccountMovims;

        public async Task<AccountResponse?> CreateAccount(CreateAccountRequest accountRequest) =>
            await _sbApiConnect.PostAndReturnAsync<AccountResponse>(_requestUri, accountRequest, true) as AccountResponse;



    }
}
