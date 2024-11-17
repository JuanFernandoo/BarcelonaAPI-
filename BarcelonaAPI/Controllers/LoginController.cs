using BarcelonaAPI.Data;
using BarcelonaAPI.Models;
using BarcelonaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BarcelonaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    { 
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        public LoginController(IUserService userService, ApplicationDbContext context)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest("Ingrese los datos requeridos"); 
            }

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Ingrese los datos requeridos");
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized("Usuario y/o contraseña incorrecta"); 
            }
            
            var token = GenerateJwtToken(user);
            return Ok(new 
            {
                message = "Login exitoso",
                username = user.Username,
                email = user.Email,
                userId = user.UserId,
                token
            });
        }

        private string GenerateJwtToken(Users user1)
        {
            var claims = new List<Claim>
            {
                new Claim("UserId", user1.UserId.ToString()),
                new Claim(ClaimTypes.Name, user1.Username),
                new Claim(ClaimTypes.Email, user1.Email)    
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Fc_B4rc3l0n4_2024_Fc_B4rc3l0n4_2024_Fc_B4rc3l0n4_2024"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7199",
                audience: "https://localhost:7199",
                claims: claims,
                expires: DateTime.Now.AddMinutes(100000),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token); 
        }

        private bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            var passwordHasher = new PasswordHasher<Users>();
            var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, plainPassword);
            return result == PasswordVerificationResult.Success;
        }

        public class LoginRequest
        {
            public string Username { get; set; }

            public string Password { get; set; }
        }

    }
}
