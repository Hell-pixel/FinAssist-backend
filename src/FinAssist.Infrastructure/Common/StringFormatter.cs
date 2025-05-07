namespace FinAssist.Infrastructure.Common;

public static class StringFormatter
{
    public static string GetEntityName(this Type type)
    {
        return type.Name.Replace("Entity", "");
    }

    public static string GetEscapedString(string str)
    {
        return $"\"{str}\"";
    }
}