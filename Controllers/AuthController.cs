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
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ResturangDbContext _db;
        private readonly JwtSettingr _jwt;
        public AuthController(ResturangDbContext db, IConfiguration cfg)
        {
            _db = db;
            _jwt = cfg.GetSection("Jwt").Get<JwtSettingr>()!;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogInRequest req)
        {
            var admin = await _db.Admins.SingleOrDefaultAsync(a => a.Username == req.Username);
            if (admin == null || !PasswordHelper.Verify(req.Password, admin.PasswordHash, admin.PasswordSalt))
                return Unauthorized("Fel användarnamn eller lösenord");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwt.ExpiresMinutes);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, admin.Id.ToString()),
                new Claim(ClaimTypes.Name, admin.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token), Expires = expires });
        }
    }
}

