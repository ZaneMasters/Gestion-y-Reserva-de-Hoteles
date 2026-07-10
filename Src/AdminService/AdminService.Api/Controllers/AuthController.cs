using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdminService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Genera un JWT para acceder a los endpoints protegidos.
    /// Usuarios demo: admin/admin123, agent/agent123
    /// </summary>
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Demo credentials — in production, validate against a real user store
        var validUsers = new Dictionary<string, (string password, string role)>
        {
            { "admin", ("admin123", "Administrator") },
            { "agent", ("agent123", "TravelAgent") }
        };

        if (!validUsers.TryGetValue(request.Username, out var userData)
            || userData.password != request.Password)
        {
            return Unauthorized(new { Message = "Credenciales inválidas." });
        }

        var token = GenerateJwt(request.Username, userData.role);
        return Ok(new { Token = token, ExpiresIn = "1 hora" });
    }

    private string GenerateJwt(string username, string role)
    {
        var key = _configuration["Jwt:Key"] ?? "AdminHotels_Super_Secret_Key_2025!";
        var issuer = _configuration["Jwt:Issuer"] ?? "AdminHotels";
        var audience = _configuration["Jwt:Audience"] ?? "AdminHotels";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public record LoginRequest(string Username, string Password);
