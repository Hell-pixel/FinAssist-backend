using System.Linq.Expressions;
using FinAssist.Domain.Entities;

namespace FinAssist.Infrastructure.Persistence.SqlBuilder;

public class QueryBuilderSelect<T> : QueryBuilderBase<T> where T : IDataBaseDocument
{
    public QueryBuilderSelect() : base()
    {
    }

    protected internal QueryBuilderSelect<T> Select<TDto>()
    {
        var properties = typeof(TDto).GetProperties();
        var fields = GetFieldNames(properties);
        return Select(fields.Select(x => $"{QueryHelper.Quote(TableNameBase)}.{QueryHelper.Quote(x)}").ToArray());
    }

    public QueryBuilderSelect<T> InnerJoin<TDataBaseDocument>(Expression<Func<T, TDataBaseDocument, bool>> expression)
    {
        return JoinBase(QueryConstants.InnerJoin, expression);
    }

    public QueryBuilderSelect<T> LeftJoin<TDataBaseDocument>(Expression<Func<T, TDataBaseDocument, bool>> expression)
    {
        return JoinBase(QueryConstants.LeftJoin, expression);
    }

    public QueryBuilderSelect<T> RightJoin<TDataBaseDocument>(Expression<Func<T, TDataBaseDocument, bool>> expression)
    {
        return JoinBase(QueryConstants.RightJoin, expression);
    }

    private QueryBuilderSelect<T> JoinBase<TDataBaseDocument>(string joinType,
        Expression<Func<T, TDataBaseDocument, bool>> expression)
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

    public QueryBuilderSelect<T> Where(Expression<Func<T, bool>> expression)
    {
        var (left, _) = ExpressionFactory.GetFieldNames(expression);
        left = PrepareString(left);
        var value = ExpressionFactory.GetValue(expression);
        var action = ExpressionFactory.GetActionAsString(expression.Body.NodeType);
        var whereString = $"{QueryHelper.Quote(TableNameBase)}.{left} {action} '{value}'";
        return Where(whereString);
    }

    protected internal QueryBuilderSelect<T> Select(params string[] queryFields)
    {
        var fields = queryFields.Length > 1 ? string.Join(",", queryFields) : queryFields.FirstOrDefault();
        Query = $"{QueryConstants.Select} {fields ?? "*"} {QueryConstants.From} {QueryHelper.Quote(TableNameBase)}";
        return this;
    }

    public QueryBuilderSelect<T> Where(string where)
    {
        Query += $" {QueryConstants.Where} {where}";
        return this;
    }

    public QueryBuilderSelect<T> Like(string value)
    {
        Query += $" {QueryConstants.Like} {value}";
        return this;
    }
}