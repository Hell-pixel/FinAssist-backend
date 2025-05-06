namespace FinAssist.Domain.Dtos.Account;

public class AccountTokenDto
{
    public string Token { get; set; }
    public string TokenType { get; set; }
    public double ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
}