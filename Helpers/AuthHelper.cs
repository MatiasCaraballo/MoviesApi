namespace MoviesApp.Helpers;
using Microsoft.AspNetCore.Identity;



public class AuthHelper
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthHelper(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    /// <summary>
    /// Creates an role for the user.
    /// </summary>
    /// <param name="role">Name of the new role</param>
    /// <returns>Response if the succes or error response</returns>
    public async Task<bool> createRole(string role, AppUser user)
    {
        //Checks if the role exists, then adds the role to the user
        bool checkRole = await _roleManager.RoleExistsAsync(role);
        if (!checkRole)
        {
            var createRole = await _roleManager.CreateAsync(new IdentityRole(role));
            if (!createRole.Succeeded) { return false; }

        }
        
        await _userManager.AddToRoleAsync(user, role);
        return true;
    }
}
