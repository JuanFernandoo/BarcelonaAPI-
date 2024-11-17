using BarcelonaAPI.Models;

namespace BarcelonaAPI.Services
{
    public interface IUserService
    {
        Task<Users> RegisterUser(string username, string password, string email);
        Task<Users> Login(string username, string password);
    }
}
