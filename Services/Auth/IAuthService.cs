public interface IAuthService
{
    Task<(bool Succeeded, string[] Errors)> RegisterAsync(RegisterDto model);
    Task<(bool Success, string? Token, DateTime? Expiration, string? Error)> LoginAsync(LoginDto model);
} 