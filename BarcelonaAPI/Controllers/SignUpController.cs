using BarcelonaAPI.Data;
using BarcelonaAPI.Dto;
using BarcelonaAPI.Models;
using BarcelonaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BarcelonaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        public SignUpController(IUserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }
        [HttpPost ("SignUp")]

        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existigEmail = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (existigEmail != null)
            {
                return BadRequest("El email esta en uso");
            }

            var existingUser = await _context.Users.SingleOrDefaultAsync (u => u.Username == request.Username);
            if (existingUser != null)
            {
                return BadRequest("El nombre de usuario esta en uso");
            }

            var user = new Users
            {
                Username = request.Username,
                Email = request.Email
            };
            user.PasswordHash = HashPassword(request.Password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new 
            {
                username = user.Username,
                email = user.Email
            });
        }

        private string HashPassword(string password)
        {
            var passwoordHasher = new PasswordHasher<Users> ();
            return passwoordHasher.HashPassword(null, password);
        }

        private bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            var passwordHasher = new PasswordHasher<Users>();
            var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, plainPassword);
            return result == PasswordVerificationResult.Success;
        }

        public class SignUpRequest
        {
            [EmailAddress(ErrorMessage = "El formato del correo electrónico es inválido.")]
            public string Email { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
