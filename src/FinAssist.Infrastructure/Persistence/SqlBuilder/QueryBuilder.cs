using FinAssist.Domain.Entities;

namespace FinAssist.Infrastructure.Persistence.SqlBuilder;

public class QueryBuilder<TEntity> where TEntity : IDataBaseDocument
{
    private QueryBuilderSelect<TEntity> QSelect { get; }
    private QueryBuilderInsert<TEntity> QInsert { get; }

    public QueryBuilder()
    {
        QSelect = new QueryBuilderSelect<TEntity>();
        QInsert = new QueryBuilderInsert<TEntity>();
    }

    public QueryBuilderSelect<TEntity> Select(params string[] queryFields)
        => QSelect.Select(queryFields);

    /*public QueryBuilderInsert<TDocument> Insert(TDocument model)
        => QInsert.Insert(model);*/
}