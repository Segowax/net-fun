namespace Repository.Repositories.Interfaces
{
    public interface IBaseRepository<Entity> where Entity : class
    {
        Task<IEnumerable<Entity>> GetAllAsync();
        Task<Entity?> GetByIdAsync(int id);
        Task<Entity?> GetByNameAsync(string name);
    }
}
