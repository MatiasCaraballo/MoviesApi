using Microsoft.AspNetCore.Identity;
using MoviesApp.Helpers;

public class AccountService : IAccountService
{
    private readonly UserManager<AppUser> _userManager;

    private readonly AuthHelper _authHelper;

    private readonly IConfiguration _configuration;


    public AccountService(
        UserManager<AppUser> userManager,
        AuthHelper authHelper,
        IConfiguration configuration



    )
    {
        _userManager = userManager;
        _authHelper = authHelper;
        _configuration = configuration;



    }
    public async Task<(bool Succeeded, string[]? Errors)> ValidateUniqueEmail(string email)
    {
        var userExists = await _userManager.FindByEmailAsync(email);
        if (userExists != null)
            return (false, new[] { "That email address is already in use." });
        else
        {
            return (true, null);
        }
    }
    public async Task<(bool Succeeded, string[]? Errors)> ValidateUniqueUserName(string username)
    {
        var userNameExists = await _userManager.FindByNameAsync(username);
        if (userNameExists != null) return (false, new[] { "That name is already in use." });
        else
        {
            return (true, null);
        }

    }

    public async Task<(bool Succeeded, string[]? Errors)> AssignRole(AppUser appUser)
    {
        try
        {
            /* To use this method asign the admin role in appsettings.json*/
            var adminEmail = _configuration["AdminSettings:Email"]; 

            if (appUser.Email == adminEmail) { var createRole = await _authHelper.createRole("admin", appUser); }
            else { var createRole = await _authHelper.createRole("currentUser", appUser); }

            return (true,null);
        }
        catch
        {
            return (false, new[] { "Error assigning the role" });
        }
        

        


    }
}