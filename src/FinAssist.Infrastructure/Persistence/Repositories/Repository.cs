using System.Data;
using Dapper;
using FinAssist.Domain.Entities;
using FinAssist.Domain.Repositories;
using FinAssist.Infrastructure.Persistence.Context;
using FinAssist.Infrastructure.Persistence.SqlBuilder;
using FinAssist.Infrastructure.Persistence.Utils;

namespace FinAssist.Infrastructure.Persistence.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IDataBaseDocument
{
    protected readonly DapperContext Context;
    protected readonly string TableName;
    protected readonly string KeyColumnName;

    public QueryBuilder<TEntity> Q { get; } = new ();

    protected Repository(DapperContext context)
    {
        Context = context;
        //TableName = DatabaseTableHelper.GetTableName<TEntity>();
        //KeyColumnName = DatabaseTableHelper.GetKeyColumnName<TEntity>();
    }

    public async Task<TEntity?> GetById(Guid id)
    {
        using var connection = Context.CreateConnection();
        var query = $"SELECT * FROM {TableName} WHERE {KeyColumnName} = @Id";
        return await connection.QuerySingleOrDefaultAsync<TEntity>(query, new { Id = id });
    }

    protected async Task<IEnumerable<TEntity>> GetList<T>(QueryBuilderBase<TEntity> builder)
        => await GetList<T>(builder.Query);

    private async Task<IEnumerable<TEntity>> GetList<T>(string query)
    {
        using var connection = Context.CreateConnection();
        return await connection.QueryAsync<TEntity>(query);
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        using var connection = Context.CreateConnection();
        var query = $"SELECT * FROM {TableName}";
        return await connection.QueryAsync<TEntity>(query);
    }

    public async Task Insert(TEntity entity)
    {
        using var connection = Context.CreateConnection();
        
        
        //var columns = string.Join(", ", DatabaseTableHelper.GetColumnNames<TEntity>());
        //var values = string.Join(", ", DatabaseTableHelper.GetParameterNames<TEntity>());

        //entity.CreatedAt = DateTime.Now;

        //var query = $"INSERT INTO {TableName} ({columns}) VALUES ({values})";
        //await connection.ExecuteAsync(query, entity);
    }

    public async Task Update(TEntity entity)
    {
        using var connection = Context.CreateConnection();

        //var setParameters = string.Join(", ", DatabaseTableHelper.GetSetParameters<TEntity>(KeyColumnName));
        
        //entity.UpdatedAt = DateTime.UtcNow;

        //var query = $"UPDATE {TableName} SET {setParameters} WHERE {KeyColumnName} = @Id";
        //await connection.ExecuteAsync(query, entity);
    }

    public async Task Delete(Guid id)
    {
        var connection = Context.CreateConnection();
        var query = $"DELETE FROM {TableName} WHERE {KeyColumnName} = @Id";
        await connection.ExecuteAsync(query, new { Id = id });
    }
}