using System.ComponentModel.DataAnnotations.Schema;

namespace FinAssist.Domain.Entities;

[Table("UserSessions")]
public class UserSessionEntity : BaseEntity
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Пользователь
    /// </summary>
    [NotMapped]
    public UserEntity? User { get; set; }

    /// <summary>
    /// Дата истечения сессии
    /// </summary>
    public DateTime ExpiresAt { get; set; }
    
    /// <summary>
    /// Устройство
    /// </summary>
    public required string UserAgent { get; set; }
    
    /// <summary>
    /// Ip адрес
    /// </summary>
    public string? IpAddress { get; set; }
    
    /// <summary>
    /// Хеш refresh токена
    /// </summary>
    public required string RefreshTokenHash { get; set; }
    
    /// <summary>
    /// Дата истечения refresh токена
    /// </summary>
    public DateTime RefreshTokenExpiresAt { get; set; }
}
