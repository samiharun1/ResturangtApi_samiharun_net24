using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using ResturangtApi_samiharun_net24.Models.Data;
using ResturangtApi_samiharun_net24.Models.Security;
using ResturangtApi_samiharun_net24.Models.Dtos;

namespace ResturangApi.Controllers
{
    [ApiController] // Markerar att detta är en API-controller
    [Route("api/[controller]")] // Base URL: /api/auth
    public class AuthController : ControllerBase
    {
        private readonly ResturangDbContext _db; // Databasen
        private readonly JwtSettingr _jwt; // Inställningar för JWT-token

        public AuthController(ResturangDbContext db, IConfiguration cfg)
        {
            _db = db;
            _jwt = cfg.GetSection("Jwt").Get<JwtSettingr>()!; // Hämtar JWT-inställningar från appsettings.json
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogInRequest req)
        {
            // Hitta admin med samma användarnamn
            var admin = await _db.Admins.SingleOrDefaultAsync(a => a.Username == req.Username);

            // Om admin inte finns eller lösenordet är fel  returnera 401 Unauthorized
            if (admin == null || !PasswordHelper.Verify(req.Password, admin.PasswordHash, admin.PasswordSalt))
                return Unauthorized("Fel användarnamn eller lösenord");

            // Skapa JWT-token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwt.ExpiresMinutes); // Tokenens giltighetstid

            // Definiera claims (info i token)
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, admin.Id.ToString()), // ID på admin
                new Claim(ClaimTypes.Name, admin.Username),                  // Användarnamn
                new Claim(ClaimTypes.Role, "Admin")                          // Roll: Admin
            };

            // Skapa token
            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            // Returnera token + giltighetstid till frontend
            return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token), Expires = expires });
        }
    }
}

