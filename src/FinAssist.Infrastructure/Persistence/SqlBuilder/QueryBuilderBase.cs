using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using FinAssist.Domain.Entities;

namespace FinAssist.Infrastructure.Persistence.SqlBuilder;

public class QueryBuilderBase<T> where T : IDataBaseDocument
{
    public string Query { get; protected set; }
    protected string TableNameBase { get; }

    protected QueryBuilderBase()
    {
        TableNameBase = GetTableName<T>();
    }

    protected string GetTableName<TDataBaseDocument>()
    {
        var attribute = typeof(TDataBaseDocument).GetCustomAttribute<TableAttribute>();
        return attribute?.Name ?? throw new Exception("Not found TableAttribute of entity");
    }

    public QueryBuilderBase<T> CustomQuery(string query)
    {
        Query = query;
        return this;
    }

    protected string[] GetFieldNames(PropertyInfo[] properties)
        => properties.Select(property => property.Name).Select(PrepareString).ToArray();

    protected string[] GetFieldAttributes(PropertyInfo[] properties)
        => properties.Select(property => property.GetCustomAttribute<ColumnAttribute>())
            .Select(columnAttribute => columnAttribute.Name).ToArray();

    protected string[] GetValues(PropertyInfo[] properties, T model)
        => properties.Select(property => property.GetValue(model, null).ToString()).ToArray();

    protected string PrepareString(string value)
    {
        return QueryHelper.ToLowerFirst(value);
    }
}