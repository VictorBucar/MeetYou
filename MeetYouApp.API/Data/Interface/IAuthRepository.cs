using System.Threading.Tasks;
using MeetYouApp.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace MeetYouApp.API.Data.Interface
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);

        SecurityTokenDescriptor GenerateToken(User userForLoginDto);
        
    }
}