using SimpleBank.AcctManage.Core.Application.Contracts.Business;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Core.Domain;
using System.Security.Cryptography;
using System.Text;

namespace SimpleBank.AcctManage.Core.Application.Business
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserBusiness(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }


        //CREATE
        public async Task<User?> CreateUser(User user, string password)
        {
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.Salt = passwordSalt;
            user.Password = passwordHash;

            var savedUser = await _unitOfWork.Users.DirectAddAsync(user);
            if (savedUser == null) { return null; }

            return savedUser;
        }

        //Login
        public bool VerifyUserCredentials(string password, string username, out Guid userId)
        {
            var user = _unitOfWork.Users.FirstOrDefault(u => u.Username == username); 
            if (user == null)
            { userId = Guid.Empty; return false; }

            if (!VerifyPasswordHash(password, user.Password, user.Salt))
            { userId = Guid.Empty; return false; }

            userId = user.Id;
            return true;
        }



        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(GenerateSalt()))
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private byte[] GenerateSalt() //my custom salt! the simplest
        {
            Random rand = new Random();
            int stringLength = rand.Next(4, 10);
            string str = "";
            char letter;

            for (int i = 0; i < stringLength; i++)
            {
                letter = Convert.ToChar(rand.Next(0, 26) + 65);
                str = str + letter;
            }
            return Encoding.UTF8.GetBytes(str);
        }


    }
}
