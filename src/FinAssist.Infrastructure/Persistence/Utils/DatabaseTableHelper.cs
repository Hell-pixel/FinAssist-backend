using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace FinAssist.Infrastructure.Persistence.Utils;

public static class DatabaseTableHelper
{
    public static string GetTableName<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);
        var tableAttr = type.GetCustomAttribute<TableAttribute>();
        if (tableAttr is not null)
        {
            return tableAttr.Name;
        }

        var className = type.Name;
        if (className.EndsWith("Entity"))
        {
            className = className.Replace("Entity", string.Empty);
        }

        var tableName = className + "s";
        return tableName;
    }
}