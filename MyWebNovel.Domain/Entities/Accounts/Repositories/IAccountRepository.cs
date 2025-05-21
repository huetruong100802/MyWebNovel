using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Domain.Entities.Accounts.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account?> GetByUsernameOrEmailAsync(string identifier);
    }
}
