namespace MyWebNovel.Domain.Entities.Roles.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(int roleId);
    }
}
