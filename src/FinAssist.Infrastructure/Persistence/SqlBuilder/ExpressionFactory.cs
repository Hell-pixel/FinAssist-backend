using System.Linq.Expressions;

namespace FinAssist.Infrastructure.Persistence.SqlBuilder;

public static class ExpressionFactory
{
    private static object GetValueBase<T>(MemberExpression member)
    {
        var objectMember = Expression.Convert(member, typeof(T));
        var getterLambda = Expression.Lambda<Func<T>>(objectMember);
        var getter = getterLambda.Compile();
        return (T)getter.Invoke()!;
    }

    public static (string, string) GetFieldNames(LambdaExpression exp)
    {
        var expression = (BinaryExpression)exp.Body;
        var left = RemoveToDot(expression.Left.ToString());
        var right = RemoveToDot(expression.Right.ToString());
        return (left, right);
    }

    public static string GetActionAsString(ExpressionType type)
    {
        return type switch
        {
            ExpressionType.Equal => "=",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            _ => ""
        };
    }

    public static object GetValue(LambdaExpression exp)
    {
        var binaryExpression = (BinaryExpression)exp.Body;
        return GetValueBase<object>((MemberExpression)binaryExpression.Right);
    }

    private static string RemoveToDot(string value)
    {
        var index = value.IndexOf('.');
        return value.Remove(0, index + 1);
    }
}