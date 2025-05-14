using System.ComponentModel.DataAnnotations.Schema;
using FinAssist.Domain.Enums;

namespace FinAssist.Domain.Entities;

public class TransactionEntity : BaseEntity
{
    /// <summary>
    /// Тип транзакции
    /// </summary>
    public TransactionType Type { get; set; }
    
    /// <summary>
    /// Сумма транзакции
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Дата транзакции
    /// </summary>
    public DateTime Date { get; set; }
    
    /// <summary>
    /// Примечание
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    public Guid CategoryId { get; set; }
    [NotMapped]
    public CategoryEntity? Category { get; set; }
    
    /// <summary>
    /// Пользователь
    /// </summary>
    public Guid UserId { get; set; }
    [NotMapped]
    public UserEntity? User { get; set; }
}