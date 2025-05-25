using System.Linq.Expressions;
using FinAssist.Domain.Entities;

namespace FinAssist.Infrastructure.Persistence.SqlBuilder;

public interface IQueryBuilderSelect
{
    
}
public class QueryBuilderSelect<TEntity> : QueryBuilderBase<TEntity> where TEntity : IDataBaseDocument
{
    public QueryBuilderSelect<TEntity> InnerJoin<TDataBaseDocument>(Expression<Func<TEntity, TDataBaseDocument, bool>> expression)
    {
        return JoinBase(QueryConstants.InnerJoin, expression);
    }

    public QueryBuilderSelect<TEntity> LeftJoin<TDataBaseDocument>(Expression<Func<TEntity, TDataBaseDocument, bool>> expression)
    {
        return JoinBase(QueryConstants.LeftJoin, expression);
    }

    public QueryBuilderSelect<TEntity> RightJoin<TDataBaseDocument>(Expression<Func<TEntity, TDataBaseDocument, bool>> expression)
    {
        return JoinBase(QueryConstants.RightJoin, expression);
    }

    private QueryBuilderSelect<TEntity> JoinBase<TDataBaseDocument>(string joinType,
        Expression<Func<TEntity, TDataBaseDocument, bool>> expression)
    {
        var joinTable = base.GetTableName<TDataBaseDocument>();
        var (left, right) = ExpressionFactory.GetFieldNames(expression);

        left = PrepareString(left);
        right = PrepareString(right);

        var action = ExpressionFactory.GetActionAsString(expression.Body.NodeType);
        Query +=
            $" {joinType} {QueryHelper.Quote(joinTable)} {QueryConstants.On} {QueryHelper.Quote(TableNameBase)}.{QueryHelper.Quote(left)} {action} {QueryHelper.Quote(joinTable)}.{QueryHelper.Quote(right)}";
        return this;
    }

    public QueryBuilderSelect<TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        var (left, _) = ExpressionFactory.GetFieldNames(expression);
        left = PrepareString(left);
        var value = ExpressionFactory.GetValue(expression);
        var action = ExpressionFactory.GetActionAsString(expression.Body.NodeType);
        var whereString = $"{QueryHelper.Quote(TableNameBase)}.{left} {action} '{value}'";
        return Where(whereString);
    }

    protected internal QueryBuilderSelect<TEntity> Select(params string[] queryFields)
    {
        var fields = queryFields.Length > 1 ? string.Join(",", queryFields) : queryFields.FirstOrDefault();
        Query = $"{QueryConstants.Select} {fields ?? "*"} {QueryConstants.From} {QueryHelper.Quote(TableNameBase)}";
        return this;
    }

    public QueryBuilderSelect<TEntity> Where(string where)
    {
        Query += $" {QueryConstants.Where} {where}";
        return this;
    }

    public QueryBuilderSelect<TEntity> Like(string value)
    {
        Query += $" {QueryConstants.Like} {value}";
        return this;
    }
}