namespace FinAssist.Domain.Services.Identity;

public interface IHashService
{
    bool VerifyPassword(string password, string hash, string saltString);
    (string hash, string salt) GenerateHash(string password);
    string GenerateRefreshToken();
    string HashRefreshToken(string token);
}