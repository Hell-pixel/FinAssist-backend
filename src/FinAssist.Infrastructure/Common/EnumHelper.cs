using System.ComponentModel.DataAnnotations;
using System.Reflection;
using FinAssist.Domain.Dtos;

namespace FinAssist.Infrastructure.Common;

public static class EnumHelper
{
    public static AvailableValueItemDto ToAvailableValueItem<TEnum>(this TEnum @enum) where TEnum : struct, Enum
    {
        return new AvailableValueItemDto(@enum, @enum.GetDisplayName());
    }
        
    public static AvailableValueItemDto[] ToAvailableValueItems<TEnum>(this IEnumerable<TEnum> enums) where TEnum : struct, Enum
    {
        return enums
            .Select(e => e.ToAvailableValueItem())
            .ToArray();
    }
    
    private static string GetDisplayName<TEnum>(this TEnum value) where TEnum : struct, Enum
    {
        return typeof(TEnum)
            .GetField(value.ToString())?
            .GetCustomAttribute<DisplayAttribute>()?
            .GetName() ?? value.ToString();
    }
}
