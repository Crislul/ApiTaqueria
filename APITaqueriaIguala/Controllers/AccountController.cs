using APITaqueriaIguala.DTOs;
using APITaqueriaIguala.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace APITaqueriaIguala.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly IConfiguration _configuration;

        // Constructor único que recibe todas las dependencias necesarias
        public AccountController(UserManager<AspNetUsers> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
                return BadRequest("Email y contraseña son obligatorios.");

            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
                return BadRequest("El usuario ya existe.");

            var newUser = new AspNetUsers
            {
                Id = Guid.NewGuid().ToString(),
                UserName = SanitizeUserName(user.Name, user.Email),
                Email = user.Email,
                Occupation = user.Occupation,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUser, user.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Usuario registrado exitosamente." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
                return Unauthorized(new { message = "Credenciales incorrectas." });

            var token = GenerateJwtToken(user);
            return Ok(new { success = true, token });
        }

        private string GenerateJwtToken(AspNetUsers user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string SanitizeUserName(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = email.Split('@')[0];

            name = Regex.Replace(name, @"[^a-zA-Z0-9]", "");

            return string.IsNullOrWhiteSpace(name) ? "User" + new Random().Next(1000, 9999) : name;
        }
    }
}