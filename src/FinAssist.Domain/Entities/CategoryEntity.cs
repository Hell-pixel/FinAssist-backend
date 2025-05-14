using System.ComponentModel.DataAnnotations.Schema;

namespace FinAssist.Domain.Entities;

public class CategoryEntity : BaseEntity
{
    /// <summary>
    /// Название категории
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Системная категория
    /// </summary>
    public bool IsSystem { get; set; }
    
    /// <summary>
    /// Пользователь
    /// </summary>
    public Guid? UserId { get; set; }
    [NotMapped]
    public UserEntity? User { get; set; }
}