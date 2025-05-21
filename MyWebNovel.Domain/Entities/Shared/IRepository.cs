namespace MyWebNovel.Domain.Entities.Shared
{
    public interface IRepository<T> where T : EntityBase
    {
        Task AddAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetSoftDeletedBeforeAsync(DateTimeOffset cutoffDate);
        void HardDelete(T entity);
    }
}
