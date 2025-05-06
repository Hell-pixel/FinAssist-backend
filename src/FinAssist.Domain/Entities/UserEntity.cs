using System.ComponentModel.DataAnnotations.Schema;

namespace FinAssist.Domain.Entities;

[Table("Users")]
public class UserEntity : BaseEntity
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Email подтвержден
    /// </summary>
    public bool EmailConfirmed { get; set; }
    
    /// <summary>
    /// Хеш
    /// </summary>
    public string Hash { get; set; }
    
    /// <summary>
    /// Соль
    /// </summary>
    public string Salt { get; set; }
    
    /// <summary>
    /// Блокировка пользователя
    /// </summary>
    public bool LockoutEnabled { get; set; }
    
    /// <summary>
    /// Дата окончания блокировки
    /// </summary>
    public DateTime? LockoutEnd { get; set; }
    
    /// <summary>
    /// Счетчик неудачных попыток входа
    /// </summary>
    public int AccessFailedCount { get; set; }
    
    /// <summary>
    /// Сессии пользователя
    /// </summary>
    [NotMapped]
    public ICollection<UserSessionEntity> Sessions { get; set; }
}
