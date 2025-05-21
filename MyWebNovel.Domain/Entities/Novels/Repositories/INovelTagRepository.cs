using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Domain.Entities.Novels.Repositories
{
    public interface INovelTagRepository : IRepository<NovelTag>
    {
        Task<NovelTag?> GetByNameAsync(string name);
    }
}
