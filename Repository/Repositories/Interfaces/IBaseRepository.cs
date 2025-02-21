namespace Repository.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity?> GetByNameAsync(string name);
        Task AddAsync(TEntity entity);
    }
}
