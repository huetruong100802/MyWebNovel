using Microsoft.EntityFrameworkCore;
using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Infrastructure.Persistence.Repositories
{
    public class Repository<T>(AppDbContext context) : IRepository<T> where T : EntityBase
    {
        private readonly AppDbContext _context = context;
        public virtual async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<IEnumerable<T>> GetSoftDeletedBeforeAsync(DateTimeOffset cutoffDate)
        {
            return await _context.Set<T>().IgnoreQueryFilters().Where(x => x.IsDeleted && x.Deleted <= cutoffDate).AsNoTracking().ToListAsync();
        }

        public virtual void HardDelete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
