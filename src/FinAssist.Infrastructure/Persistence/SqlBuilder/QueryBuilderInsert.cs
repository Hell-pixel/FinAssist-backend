using System.ComponentModel.DataAnnotations;
using System.Reflection;
using FinAssist.Domain.Entities;

namespace FinAssist.Infrastructure.Persistence.SqlBuilder;

public class QueryBuilderInsert<T> : QueryBuilderBase<T> where T : IDataBaseDocument
{
    public QueryBuilderInsert() : base()
    {
    }

    public QueryBuilderInsert<T> Insert(T model)
    {
        var properties = model.GetType().GetProperties();
        properties = properties.Where(x => x.GetCustomAttribute(typeof(KeyAttribute)) == null).ToArray();

        var fieldAttributes = GetFieldAttributes(properties);
        var values = GetValues(properties, model);
        return Insert(fieldAttributes).Values(values);
    }

    public QueryBuilderInsert<T> Insert(params string[] parameters)
    {
        Query =
            $"{QueryConstants.InsertInto} {QueryHelper.Quote(TableNameBase)} {QueryHelper.MapParametersQuote('"', parameters)}";
        return this;
    }

    public QueryBuilderInsert<T> Values(params string[] values)
    {
        Query += $" {QueryConstants.Values} {QueryHelper.MapParametersQuote('\'', values)}";
        return this;
    }
}