public interface IAccountService
{
    Task<(bool Succeeded, string[]? Errors)> ValidateUniqueEmail(string email);
    Task<(bool Succeeded, string[]? Errors)> ValidateUniqueUserName(string username);
    Task<(bool Succeeded, string[]? Errors)> AssignRole(AppUser appUser);

    Task<(bool Succeeded, string[]? Errors,AppUser? appUser)> ValidateEmailExists(string email); 



}