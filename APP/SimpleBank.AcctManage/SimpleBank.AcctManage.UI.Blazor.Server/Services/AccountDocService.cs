using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SimpleBank.AcctManage.UI.Blazor.Server.Data;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Mapper;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Responses;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Services
{
    public class AccountDocService
    {
        private readonly SimpleBankClient _client;
        private readonly EntityMapper _mapper;

        private readonly string _baseUri = "/api/v2/accounts/";

        public AccountDocService(
            SimpleBankClient client,
            EntityMapper mapper)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<IEnumerable<AccountDoc>?> GetAll(Guid accountId)
        {
            var requestUri = _baseUri + accountId.ToString() + "/GetAllDocs";
            var rspAccountDocs = await _client.GetAsync<IEnumerable<ResponseAccountDoc>>(requestUri, true);
            if (rspAccountDocs == null) { return null; }
            return _mapper.Map(rspAccountDocs);
        }
        public async Task<AccountDoc?> Get(Guid accountId, Guid docId)
        {
            var requestUri = _baseUri + accountId.ToString() + "/GetDoc/" + docId.ToString();
            var rspDoc = await _client.GetAsync<ResponseAccountDoc>(requestUri, true);
            if (rspDoc == null) { return null; }
            return _mapper.Map(rspDoc);
        }

        public async Task<string?> Download(Guid accountId, Guid docId, string docType)
        {
            var requestUri = _baseUri + accountId.ToString() + "/Download/" + docId.ToString();
            var rsp = await _client.GetContentAsync(requestUri, true);

            var rspBt = await rsp.ReadAsByteArrayAsync();
            var rspStringed = Convert.ToBase64String(rspBt);
            var imageDataURL = string.Format($"data:{docType};base64,{rspStringed}");

            return imageDataURL;
        }

        public async Task<bool> Upload(Guid accountId, IBrowserFile file)
        {
            var requestUri = _baseUri + accountId.ToString() + "/Upload";
            var rsp = await _client.PostFileAsync(requestUri, file, true);
            return rsp;
        }







    }
}

