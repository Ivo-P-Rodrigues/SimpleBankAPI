using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleBankAPI.Contracts;
using SimpleBankAPI.Models;
using SimpleBankAPI.Repository;
using SimpleBankAPI.Services;

namespace SimpleBankAPI.Business
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenGenerator _tokenGenerator;

        private readonly ILogger _logger;
        public UserBusiness(
            IUnitOfWork unitOfWork,
            ITokenGenerator tokenGenerator,
            ILogger<UserBusiness> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
        }


        public async Task<CreateUserResponse?> Create(CreateUserRequest createUserRequest)
        {
            User newUser = _unitOfWork.HardEntityMapper.MapRequestToUser(createUserRequest);

            try
            {
                _unitOfWork.Users.Add(newUser);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Critical error on user creation.");
                return null;
            }

            return _unitOfWork.HardEntityMapper.MapUserToResponse(newUser);
        }


        public async Task<string?> ProcessLogin(LoginUserRequest loginUserRequest)
        {
            User? user = await GetUserByUsernamePasswordComboAsync(loginUserRequest);
            if (user == null) { return null; }

            return _tokenGenerator.GenerateToken(user);
        }



        public bool CheckUsernamePasswordCombo(LoginUserRequest loginUserRequest) =>
            _unitOfWork.Users.Exists(user => user.Username == loginUserRequest.Username && user.Password == loginUserRequest.Password);
        public User? GetUserByUsernamePasswordCombo(LoginUserRequest loginUserRequest) =>
            _unitOfWork.Users.Get(user => user.Username == loginUserRequest.Username && user.Password == loginUserRequest.Password);
        public async Task<User?> GetUserByUsernamePasswordComboAsync(LoginUserRequest loginUserRequest) =>
            await _unitOfWork.Users.GetAsync(user => user.Username == loginUserRequest.Username && user.Password == loginUserRequest.Password);







    }
}
