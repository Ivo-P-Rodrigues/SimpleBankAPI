using SimpleBankAPI.Models;

namespace SimpleBankAPI.Services
{
    public interface ITokenGenerator
    {
        string GenerateToken(User user);
    }
}