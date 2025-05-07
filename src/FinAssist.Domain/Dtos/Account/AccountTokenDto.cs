namespace FinAssist.Domain.Dtos.Account;

public class AccountTokenDto
{
    /// <summary>
    /// Токен доступа
    /// </summary>
    public required string Token { get; set; }
    
    /// <summary>
    /// Тип токена
    /// </summary>
    public required string TokenType { get; set; }
    
    /// <summary>
    /// Время жизни токена в секундах
    /// </summary>
    public double ExpiresIn { get; set; }
    
    /// <summary>
    /// Токен для обновления доступа
    /// </summary>
    public required string RefreshToken { get; set; }
    
    /// <summary>
    /// Время жизни токена обновления в секундах
    /// </summary>
    public double RefreshTokenExpiresIn { get; set; }
}