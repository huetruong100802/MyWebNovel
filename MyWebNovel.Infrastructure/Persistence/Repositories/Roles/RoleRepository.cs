using MyWebNovel.Domain.Entities.Roles;
using MyWebNovel.Domain.Entities.Roles.Repositories;

namespace MyWebNovel.Infrastructure.Persistence.Repositories.Roles
{
    public class RoleRepository(AppDbContext context) : IRoleRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Role?> GetByIdAsync(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }
    }
}
