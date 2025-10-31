using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sudoku.Api.Models;
using Sudoku.Core.Entities;
using Sudoku.Core.Repositories;
using Sudoku.Core.Security;

namespace Sudoku.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IConfiguration configuration,
        ILogger<AuthController> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        // Validate email
        if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
        {
            return BadRequest(new { error = "Invalid email address" });
        }

        // Validate password
        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
        {
            return BadRequest(new { error = "Password must be at least 8 characters long" });
        }

        // Check if user already exists
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return BadRequest(new { error = "User with this email already exists" });
        }

        // Create new user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            DisplayName = request.DisplayName ?? request.Email.Split('@')[0],
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(user);

        // Log with sanitized email (remove control characters to prevent log forging)
        var sanitizedEmail = SanitizeForLogging(user.Email);
        _logger.LogInformation("User registered: {Email}", sanitizedEmail);

        // Generate JWT token
        var token = GenerateJwtToken(user);

        var response = new AuthResponse
        {
            UserId = user.Id,
            Email = user.Email,
            DisplayName = user.DisplayName,
            Token = token
        };

        return CreatedAtAction(nameof(Register), response);
    }

    /// <summary>
    /// Login user
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { error = "Email and password are required" });
        }

        // Find user
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            return Unauthorized(new { error = "Invalid email or password" });
        }

        // Verify password
        if (!_passwordHasher.VerifyPassword(user.PasswordHash, request.Password))
        {
            return Unauthorized(new { error = "Invalid email or password" });
        }

        // Log with sanitized email (remove control characters to prevent log forging)
        var sanitizedEmail = SanitizeForLogging(user.Email);
        _logger.LogInformation("User logged in: {Email}", sanitizedEmail);

        // Generate JWT token
        var token = GenerateJwtToken(user);

        var response = new AuthResponse
        {
            UserId = user.Id,
            Email = user.Email,
            DisplayName = user.DisplayName,
            Token = token
        };

        return Ok(response);
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSecret = _configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "SudokuApi";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "SudokuClient";
        var jwtExpiryMinutes = int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("displayName", user.DisplayName ?? string.Empty)
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtExpiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static string SanitizeForLogging(string value)
    {
        // Remove control characters (including newlines) to prevent log forging
        return new string(value.Where(c => !char.IsControl(c)).ToArray());
    }
}
