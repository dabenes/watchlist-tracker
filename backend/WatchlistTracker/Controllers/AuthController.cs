using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WatchlistTracker.Data;
using WatchlistTracker.Models;
using BCrypt.Net;

namespace WatchlistTracker.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] Usuario user)
        {
            if (_context.Usuarios.Any(u => u.Username == user.Username))
            {
                return BadRequest(new { message = "El usuario ya existe" });
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash, 11);
            
            _context.Usuarios.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "Usuario registrado correctamente" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new { message = "El usuario y la contraseña son obligatorios." });
            }

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Username == loginRequest.Username);
            if (usuario == null)
            {
                return Unauthorized(new { message = "Credenciales incorrectas." });
            }

            // 🔍 **Depuración: Compara la contraseña ingresada con el hash en la BD**
            Console.WriteLine($"Contraseña ingresada: {loginRequest.Password}");
            Console.WriteLine($"Hash en la BD: {usuario.PasswordHash}");

            string generatedHash = BCrypt.Net.BCrypt.HashPassword(loginRequest.Password);
            Console.WriteLine($"Hash generado con la contraseña ingresada: {generatedHash}");

            bool esValida = BCrypt.Net.BCrypt.Verify(loginRequest.Password, usuario.PasswordHash);
            Console.WriteLine($"¿La contraseña ingresada es válida? {esValida}");

            if (!esValida)
            {
                return Unauthorized(new { message = "Credenciales incorrectas." });
            }

            var token = GenerateJwtToken(usuario);
            return Ok(new { token });
        }

        private string GenerateJwtToken(Usuario user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [Authorize]
        [HttpGet("protected")]
        public IActionResult ProtectedRoute()
        {
            return Ok(new { message = "Acceso permitido. Usuario autenticado." });
        }
    }
}
