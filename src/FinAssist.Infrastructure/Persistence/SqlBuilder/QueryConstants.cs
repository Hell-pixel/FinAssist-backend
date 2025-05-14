namespace FinAssist.Infrastructure.Persistence.SqlBuilder;

public class QueryConstants
{
    public static readonly string InnerJoin = "INNER JOIN";
    public static readonly string LeftJoin = "LEFT JOIN";
    public static readonly string RightJoin = "RIGHT JOIN";
    public static readonly string InsertInto = "INSERT INTO";

    public static readonly string Select = nameof(Select).ToUpper();
    public static readonly string Where = nameof(Where).ToUpper();
    public static readonly string Like = nameof(Like).ToUpper();
    public static readonly string From = nameof(From).ToUpper();
    public static readonly string As = nameof(As).ToUpper();
    public static readonly string On = nameof(On).ToUpper();
    public static readonly string Values = nameof(Values).ToUpper();
}
