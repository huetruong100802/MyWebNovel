using MyWebNovel.Application.DTOs.Novel;
using MyWebNovel.Application.Pagination;

namespace MyWebNovel.Application.Interfaces
{
    public interface INovelService
    {
        Task<NovelDto> CreateNovelAsync(CreateNovelDto command);
        Task<PaginatedResult<NovelDto>> GetFilteredNovelsAsync(PaginationParams pagination, NovelFilterDto novelFilter);
        Task<NovelDto?> GetNovelByIdAsync(Guid novelId);
        Task ModifyTagAsync(Guid novelId, string TagName, bool addTag);
        Task UpdateNovelAsync(Guid id, NovelUpdateDto input);
        Task DeleteNovelAsync(Guid id);
    }
}
