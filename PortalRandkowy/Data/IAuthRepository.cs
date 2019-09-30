using PortalRandkowy.Models;
using System.Threading.Tasks;

namespace PortalRandkowy.Data
{
    public interface IAuthRepository
    {
        Task<User> Login(string username, string password);
        Task<User> Register(User user, string password);
        Task<bool> UserExists(string username);
    }
}
