using FinAssist.Infrastructure.Common;

namespace FinAssist.Infrastructure.Exceptions;

public class NotFoundException : DataException
{
    public NotFoundException() : base("Not found")
    {
    }

    public NotFoundException(Type type) : base($"{type.GetEntityName()} not found")
    {
    }

    public NotFoundException(Type type, Guid id) : base($"{type.GetEntityName()} with id {id} not found")
    {
    }

    public static NotFoundException With<T>(Guid id)
    {
        return new NotFoundException(typeof(T), id);
    }
}