using MyWebNovel.Domain.Entities.Novels.Filters;
using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Domain.Entities.Novels.Repositories
{
    public interface INovelRepository : IRepository<Novel>
    {
        Task<IEnumerable<Novel>> GetFilteredNovelsAsync(NovelFilter novelFilter);
    }
}
