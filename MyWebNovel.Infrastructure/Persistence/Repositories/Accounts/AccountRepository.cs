using Microsoft.EntityFrameworkCore;
using MyWebNovel.Domain.Entities.Accounts;
using MyWebNovel.Domain.Entities.Accounts.Repositories;

namespace MyWebNovel.Infrastructure.Persistence.Repositories.Accounts
{
    public class AccountRepository(AppDbContext context) : Repository<Account>(context), IAccountRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Account?> GetByUsernameOrEmailAsync(string identifier)
        {
            return (await _context.Accounts
                .AsNoTracking()
                .ToListAsync())
                .FirstOrDefault(a => string.Equals(a.Username, identifier, StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(a.Email.Value, identifier, StringComparison.InvariantCultureIgnoreCase));
        }

    }
}
