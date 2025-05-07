namespace FinAssist.Infrastructure.Exceptions;

public class DataException : BusinessException
{
    public DataException(string message) : base(message)
    {
    }
}