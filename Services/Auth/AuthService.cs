using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;

    private readonly IConfiguration _configuration;
    private readonly IAccountService _iAccountService;

    private readonly IClaimService _iClaimService;

    public AuthService(
        UserManager<AppUser> userManager,
        IConfiguration configuration,
        IAccountService iAccountService,
        IClaimService iClaimService
        )
    {
        _userManager = userManager;
        _configuration = configuration;
        _iAccountService = iAccountService;
        _iClaimService = iClaimService;
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

    public async Task<(bool Success, string? Token, DateTime? Expiration, string? Error)> LoginAsync(LoginDto model)
    {

        /* Validates the email  and password*/
        var userExists = await _iAccountService.ValidateEmailExists(model.Email);
        if (!userExists.Succeeded) { return (Success: false, Token: null, Expiration: null, Error: "The mail does not exists"); }
        ;

        var user = userExists.appUser;
        if (!await _userManager.CheckPasswordAsync(user, model.Password))
            return (Success: false, Token: null, Expiration: null, Error: "Invalid password.");

        /*Creates the claims*/
        var createClaims =  await _iClaimService.CreateClaims(user.Id, user.Email);
        if (!createClaims.Succeeded){ return (Success: false, Token: null, Expiration: null, Error: "Error creating the claims");}
        List<Claim> claims = createClaims.claims;

        /*Generates the token*/

        var secret = _configuration["Jwt:Secret"];

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

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


}
