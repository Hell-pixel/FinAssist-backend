
namespace FinAssist.Domain.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetById(Guid id);
    Task<IEnumerable<TEntity>> GetAll();
    Task Insert(TEntity entity);
    Task Update(TEntity entity);
    Task Delete(Guid id);
}