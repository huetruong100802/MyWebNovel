using MyWebNovel.Domain.Entities.Accounts.Repositories;
using MyWebNovel.Domain.Entities.Novels.Repositories;
using MyWebNovel.Domain.Entities.Roles.Repositories;
using MyWebNovel.Domain.Entities.Shared;
using MyWebNovel.Domain.Entities.Tokens.Repositories;

namespace MyWebNovel.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }
        IRoleRepository Roles { get; }
        INovelRepository Novels { get; }
        INovelTagRepository Tags { get; }
        IRefreshTokenRepository RefreshTokens { get; }

        IRepository<T> GetRepository<T>() where T : EntityBase;
        Task SaveChangesAsync();
    }
}
