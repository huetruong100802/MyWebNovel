using MyWebNovel.Application.Interfaces;
using MyWebNovel.Domain.Entities.Accounts.Repositories;
using MyWebNovel.Domain.Entities.Novels.Repositories;
using MyWebNovel.Domain.Entities.Roles.Repositories;
using MyWebNovel.Domain.Entities.Shared;
using MyWebNovel.Domain.Entities.Tokens.Repositories;
using MyWebNovel.Infrastructure.Persistence.Repositories;
using MyWebNovel.Infrastructure.Persistence.Repositories.Accounts;
using MyWebNovel.Infrastructure.Persistence.Repositories.Novels;
using MyWebNovel.Infrastructure.Persistence.Repositories.Roles;
using MyWebNovel.Infrastructure.Persistence.Repositories.Tokens;

namespace MyWebNovel.Infrastructure.Persistence
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        public IAccountRepository Accounts { get; } = new AccountRepository(context);

        public IRoleRepository Roles { get; } = new RoleRepository(context);

        public INovelRepository Novels { get; } = new NovelRepository(context);

        public INovelTagRepository Tags { get; } = new NovelTagRepository(context);

        public IRefreshTokenRepository RefreshTokens => new RefreshTokenRepository(context);

        public void Dispose() => context.Dispose();

        public IRepository<T> GetRepository<T>() where T : EntityBase => new Repository<T>(context);

        public async Task SaveChangesAsync() => await context.SaveChangesAsync();
    }
}
