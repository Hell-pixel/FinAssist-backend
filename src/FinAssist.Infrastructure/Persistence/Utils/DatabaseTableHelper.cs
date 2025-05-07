using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using FinAssist.Domain.Entities;
using static FinAssist.Infrastructure.Common.StringFormatter;

namespace FinAssist.Infrastructure.Persistence.Utils;

public static class DatabaseTableHelper
{
    public static string GetTableName<TEntity>() where TEntity : BaseEntity
    {
        var type = typeof(TEntity);
        var tableAttr = type.GetCustomAttribute<TableAttribute>();
        var tableName = tableAttr?.Name ?? GetDefaultTableName(type);

        return GetEscapedString(tableName);
    }

    public static string GetKeyColumnName<TEntity>() where TEntity : BaseEntity
    {
        var keyProperty = typeof(TEntity).GetProperties()
            .FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);

        var keyName = keyProperty?.Name ?? "Id";
        return GetEscapedString(keyName);
    }

    public static List<string> GetColumnNames<TEntity>(string? prefix = null) where TEntity : BaseEntity
    {
        var properties = typeof(TEntity).GetProperties()
            .Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any());

        return string.IsNullOrWhiteSpace(prefix) 
            ? properties.Select(p => GetEscapedString(p.Name)).ToList() 
            : properties.Select(p => $"{prefix}.{GetEscapedString(p.Name)}").ToList();
    }

    public static List<string> GetParameterNames<TEntity>() where TEntity : BaseEntity
    {
        var properties = typeof(TEntity).GetProperties()
            .Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any());

        return properties.Select(p => $"@{p.Name}").ToList();
    }
    
    public static List<string> GetSetParameters<TEntity>(string keyColumnName) where TEntity : BaseEntity
    {
        // Получаем оригинальные свойства без кавычек и без @
        var properties = typeof(TEntity).GetProperties()
            .Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any())
            .Select(p => p.Name)
            .Where(name => !string.Equals(name, keyColumnName, StringComparison.OrdinalIgnoreCase))
            .ToList();
        
        return properties
            .Select(p=> $"{GetEscapedString(p)} = @{p}")
            .ToList();
    }
    
    // todo:: возможно добавить извлечение из атрибута ColumnAttribute для параметров
    public static string GetColumnName<TEntity>(Expression<Func<TEntity, object>> propertyExpression) where TEntity : BaseEntity
    {
        // Извлекаем MemberExpression из выражения
        var memberExpression = propertyExpression.Body switch
        {
            MemberExpression m => m,  // Если это MemberExpression, используем его
            UnaryExpression { Operand: MemberExpression m } => m, // Если это UnaryExpression с Operand как MemberExpression
            _ => throw new InvalidOperationException("Invalid property expression") // Если это не поддерживаемый тип
        };

        return GetEscapedString(memberExpression.Member.Name);
    }

    private static string GetDefaultTableName(Type type)
    {
        var className = type.Name;
        if (className.EndsWith("Entity"))
        {
            className = className.Replace("Entity", string.Empty);
        }

        return className + "s";
    }
}
