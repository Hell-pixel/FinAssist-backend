namespace FinAssist.Domain.Dtos;

public class AvailableValueItemDto
{
    public AvailableValueItemDto(object value, string name)
    {
        Value = value;
        Name = name;
    }
    
    /// <summary> 
    /// Значение
    /// </summary>
    public object Value { get; set; }
    
    /// <summary>
    /// Значение для отображения
    /// </summary>
    public string Name { get; set; }
}