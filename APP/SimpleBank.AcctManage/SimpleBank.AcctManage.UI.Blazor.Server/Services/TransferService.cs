using SimpleBank.AcctManage.UI.Blazor.Server.Data;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Mapper;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Services
{
    public class TransferService
    {

        private readonly SimpleBankClient _client;
        private readonly EntityMapper _mapper;

        private readonly string _requestUri = "/api/v2/transfers/";

        public TransferService(
            SimpleBankClient client,
            EntityMapper mapper)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<bool> Transfer(TransferCreate transferCreate)
        {
            var requestTransfer = _mapper.Map(transferCreate);
            return await _client.PostAsync(_requestUri, requestTransfer, true);
        }







    }
}








/*
        private readonly IHttpClientFactory _client;
        private readonly ProtectedLocalStorage _localStorage;
        private readonly ILogger<TransferService> _logger;

        public TransferService(
            IHttpClientFactory client,
            ProtectedLocalStorage localStorage,
            ILogger<TransferService> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> MakeTransfer(RequestTransferCreate requestTransfer)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _requestUri);
            request.Content = JsonContent.Create(requestTransfer);
            var client = _client.CreateClient("SbApi");

            var accessToken = (await _localStorage.GetAsync<string>("accessToken")).Value;
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                _logger.LogCritical("MakeTransfer failed. Access token not found.");
                return false;
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

            var response = await client.SendAsync(request);
            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Transfer not executed. Status code returned: {response?.StatusCode}");
            }

            return response!.IsSuccessStatusCode;
        }
*/