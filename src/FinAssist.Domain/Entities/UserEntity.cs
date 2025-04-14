using System.ComponentModel.DataAnnotations.Schema;

namespace FinAssist.Domain.Entities;

[Table("Users")]
public class UserEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}