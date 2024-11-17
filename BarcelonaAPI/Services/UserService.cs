using BarcelonaAPI.Data;
using BarcelonaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BarcelonaAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        { 
            _context = context;
        }
        public async Task<Users> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if ( user == null || !VerifyPasswordHash(password, user.PasswordHash))
                    {
                return null; 
            }
            return user;
        }

        public async Task<Users> RegisterUser(string username, string password, string email)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (existingUser == null)
            {
                throw new Exception("El nombre de usuario ya está en uso."); 

            }

            var user = new Users
            {
                Username = username,
                Email = email,
                PasswordHash = HashPassword(password),
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private string HashPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }

        private bool VerifyPasswordHash (string password, string storedHash)
        {
            var hashBytes = Convert.FromBase64String(storedHash);
            using (var hmac = new HMACSHA512(hashBytes.Take(64).ToArray()))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(hashBytes.Skip(64).ToArray());
            }
        }
    }
}
