namespace GamesLand.Core;

public interface IRepository<TPrimaryKey, TEntity> where TEntity : class
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> GetByIdAsync(TPrimaryKey id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> UpdateAsync(TPrimaryKey id, TEntity entity);
    Task DeleteAsync(TPrimaryKey id);
}