using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using FinAssist.Domain.Entities;

namespace FinAssist.Infrastructure.Persistence.SqlBuilder;

public class QueryBuilderBase<TEntity> where TEntity : IDataBaseDocument
{
    public string Query { get; protected set; }
    protected string TableNameBase { get; }

    protected QueryBuilderBase()
    {
        TableNameBase = GetTableName<TEntity>();
    }

    protected string GetTableName<TDataBaseDocument>()
    {
        var attribute = typeof(TDataBaseDocument).GetCustomAttribute<TableAttribute>();
        return attribute?.Name ?? throw new Exception("Not found TableAttribute of entity");
    }

    public QueryBuilderBase<TEntity> CustomQuery(string query)
    {
        Query = query;
        return this;
    }

    protected string[] GetFieldNames(PropertyInfo[] properties)
        => properties.Select(property => property.Name).Select(PrepareString).ToArray();

    protected string[] GetFieldAttributes(PropertyInfo[] properties)
        => properties.Select(property => property.GetCustomAttribute<ColumnAttribute>())
            .Select(columnAttribute => columnAttribute.Name).ToArray();

    protected string[] GetValues(PropertyInfo[] properties, TEntity model)
        => properties.Select(property => property.GetValue(model, null).ToString()).ToArray();

    protected string PrepareString(string value)
    {
        return QueryHelper.ToLowerFirst(value);
    }
}