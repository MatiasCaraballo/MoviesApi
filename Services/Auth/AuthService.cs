using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.Swift;
using System.Security.Claims;
using System.Text;


public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;

    private readonly IConfiguration _configuration;
    private readonly IAccountService _iAccountService;

    public AuthService(
        UserManager<AppUser> userManager,
        IConfiguration configuration,
        IAccountService iAccountService
        )
    {
        _userManager = userManager;
        _configuration = configuration;
        _iAccountService = iAccountService;
    }
    /// <summary>
    /// Register a new User
    /// </summary>
    /// <param name="userModel">Object with the data to create the user </param>
    /// <returns>Response if the succes or error response</returns>
    public async Task<(bool Succeeded, string[] Errors)> RegisterAsync(RegisterDto userModel)
    {
        /*Validates that the user email and name is unique*/

        await _iAccountService.ValidateUniqueEmail(userModel.Email);

        await _iAccountService.ValidateUniqueUserName(userModel.UserName);

        /*Gets the data from the user and assings a role*/
        var user = new AppUser
        {
            UserName = userModel.UserName,
            Email = userModel.Email
        };

        await _iAccountService.AssignRole(user);

        /*Saves the user in the database*/
        var createUser = await _userManager.CreateAsync(user, userModel.Password);
        return (createUser.Succeeded, createUser.Errors.Select(e => e.Description).ToArray());


    }

    public async Task<(bool Success, string Token, DateTime? Expiration, string Error)> LoginAsync(LoginDto model)
    {
        /* Checks if the user Email and Password exists */
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return (Success: false, Token: null, Expiration: null, Error: "Invalid email or password.");

        /*Creates the claims*/
        var createClaims = CreateClaims(user.Id, model.Email);
        List<Claim> claims = createClaims.claims;


        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.UtcNow.AddHours(3),
            claims: claims,
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

    public (bool Succeeded, string?[] Errors, List<Claim> claims) CreateClaims(string id,string email) {

        try
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            return (true, null, claims);
        }
        catch
        {
            return (false, new[] { "Error creating the claims" }, null);

        }
    }
}
