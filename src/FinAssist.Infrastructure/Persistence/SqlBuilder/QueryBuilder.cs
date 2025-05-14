using FinAssist.Domain.Entities;

namespace FinAssist.Infrastructure.Persistence.SqlBuilder;

public class QueryBuilder<TDocument> where TDocument : IDataBaseDocument  
{
    private QueryBuilderSelect<TDocument> QSelect { get; }
    private QueryBuilderInsert<TDocument> QInsert { get; }

    public QueryBuilder()
    {
        QSelect = new QueryBuilderSelect<TDocument>();
        QInsert = new QueryBuilderInsert<TDocument>();
    }

    public QueryBuilderSelect<TDocument> Select<TDto>()
        => QSelect.Select<TDto>();

    public QueryBuilderSelect<TDocument> Select(params string[] queryFields)
        => QSelect.Select(queryFields);

    public QueryBuilderInsert<TDocument> Insert(TDocument model)
        => QInsert.Insert(model);
}