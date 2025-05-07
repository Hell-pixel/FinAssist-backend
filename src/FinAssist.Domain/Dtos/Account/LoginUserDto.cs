namespace FinAssist.Domain.Dtos.Account;

public class LoginUserDto
{
    /// <summary>
    /// Почта пользователя
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }
}