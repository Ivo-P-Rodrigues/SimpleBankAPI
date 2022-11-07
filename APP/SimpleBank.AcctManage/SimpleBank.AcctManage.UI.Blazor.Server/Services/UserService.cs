using SimpleBank.AcctManage.UI.Blazor.Server.Contracts.Clients;
using SimpleBank.AcctManage.UI.Blazor.Server.Contracts.Mapper;
using SimpleBank.AcctManage.UI.Blazor.Server.Contracts.Services;
using SimpleBank.AcctManage.UI.Blazor.Server.Data;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Mapper;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Services
{
    public class UserService : IUserService
    {
        private readonly ISimpleBankClient _client;
        private readonly IEntityMapper _mapper;

        private readonly string _requestUri = "/api/v2/users/";

        public UserService(
            ISimpleBankClient client,
            IEntityMapper mapper)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> Create(UserCreate createUserRequest)
        {
            var requestUserCreate = _mapper.Map(createUserRequest);
            return await _client.PostAsync(_requestUri + "create", requestUserCreate);
        }





    }
}









