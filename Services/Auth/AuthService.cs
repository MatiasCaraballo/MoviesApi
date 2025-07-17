using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<AppUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }
    /// <summary>
    /// Register a new User
    /// </summary>
    /// <param name="userModel">Object with the data to create the user </param>
    /// <returns>Response if the succes or error response</returns>
    public async Task<(bool Succeeded, string[] Errors)> RegisterAsync(RegisterDto userModel)
    {
        /*Validates that the user email and name is unique*/
        var userExists = await _userManager.FindByEmailAsync(userModel.Email);
        if (userExists != null)
            return (false, new[] { "That email address is already in use." });

        var userNameExists = await _userManager.FindByNameAsync(userModel.UserName);
        if (userNameExists != null)
            return (false, new[] { "That name is already in use." });

        /*Create the user*/
        var user = new AppUser
        {
            UserName = userModel.UserName,
            Email = userModel.Email
        };

        var result = await _userManager.CreateAsync(user, userModel.Password);
        return (result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
    }

    public async Task<(bool Success, string Token, DateTime? Expiration, string Error)> LoginAsync(LoginDto model)
    {
        /* Checks if the user Email and Password exists */
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return (Success: false, Token: null, Expiration: null, Error: "Invalid email or password.");

        var authClaims = new List<Claim>

        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.UtcNow.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
        return (
            Success: true,
            Token: tokenStr,
            Expiration: token.ValidTo,
            Error: null
        );

    }
}