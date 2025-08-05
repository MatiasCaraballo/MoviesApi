using System.Security.Claims;

public class ClaimService : IClaimService
{
    
    private readonly IUserRepository _userRepository;
     public ClaimService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<(bool Succeeded, string?[] Errors, List<Claim>? claims)> CreateClaims(string id, string email)
    {
        try
        {

            var getUserRole = await _userRepository.GetUserRole(id);
            if (getUserRole == null)
            {
                return (false, new[] { $"There is no role for the user Id {id}"}, null);
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, id),
                    new Claim(ClaimTypes.Email, email),
                    new Claim("Roles","admin"),
                };

            return (true, null, claims);


        }
        catch
        {
            return (false, new[] { "Error creating the claims" }, null);

        }
    }
}
