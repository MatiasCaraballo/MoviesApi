using System.Security.Claims;

public interface IClaimService
{
    Task<(bool Succeeded, string[]?Errors, List<Claim>? claims)> CreateClaims(string id, string email);

}