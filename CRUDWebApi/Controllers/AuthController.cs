using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IConfiguration _config;

	public AuthController(IConfiguration config)
	{
		_config = config;
	}

	[HttpPost("login")]
	public IActionResult Login([FromBody] LoginRequest request)
	{
		// only for test, because we don;t have db now!!!!!!!

		if (request.Username != "admin" || request.Password != "password")
			return Unauthorized("Неверный логин или пароль");

		var token = GenerateToken(request.Username);
		return Ok(new { token });
	}

	private string GenerateToken(string username)
	{
		var key = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var claims = new[]
		{
			new Claim(ClaimTypes.Name, username),
			new Claim(ClaimTypes.Role, "Admin")
		};

		var token = new JwtSecurityToken(
			issuer: "myapp",
			audience: "myusers",
			claims: claims,
			expires: DateTime.UtcNow.AddHours(1),
			signingCredentials: creds
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}

public record LoginRequest(string Username, string Password);