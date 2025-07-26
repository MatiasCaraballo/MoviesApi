using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

public class ClaimService : IClaimService
{
    public (bool Succeeded, string?[] Errors, List<Claim> claims) CreateClaims(string id, string email)
    {

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
