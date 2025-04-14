using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Dapper;
using FinAssist.Domain.Repositories;
using FinAssist.Infrastructure.Persistence.Context;
using FinAssist.Infrastructure.Persistence.Utils;

namespace FinAssist.Infrastructure.Persistence.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DapperContext _context;
    private readonly string _tableName;
    private readonly string _keyColumnName;

    protected Repository(DapperContext context)
    {
        _context = context;
        _tableName = DatabaseTableHelper.GetTableName<TEntity>();
    }

    public Task<TEntity> GetById(Guid id)
    {
        using var connection = _context.CreateConnection();
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task Insert(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task Update(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }


}