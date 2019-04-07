using System.Threading.Tasks;
using MeetYouApp.API.Models;

namespace MeetYouApp.API.Data.Interface
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
        
    }
}