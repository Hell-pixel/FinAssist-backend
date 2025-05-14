namespace FinAssist.Infrastructure.Persistence.SqlBuilder;

public static class QueryHelper
{
    public static string ToLowerFirst(string value)
        => char.ToLower(value[0]) + value.Substring(1);
        
    public static string Quote(string value, char quoteType = '"') 
        => $"{quoteType}{value}{quoteType}";

    public static string Mapper(params string[] values)
    {
        return string.Join(" ", values);
    }

    public static string MapParameters(params string[] values)
        => $"({string.Join(",", values)})";
        
    public static string MapParametersQuote(char typeQuote, params string[] values)
        => MapParameters(values.Select(x => $"{typeQuote}{x}{typeQuote}").ToArray());
}
