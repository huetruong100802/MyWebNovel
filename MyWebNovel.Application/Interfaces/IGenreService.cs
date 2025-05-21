
using MyWebNovel.Application.DTOs.NovelTag;

namespace MyWebNovel.Application.Interfaces
{
    public interface ITagService
    {
        Task<TagDto> CreateTagAsync(TagCreateDto newTag);
        Task DeleteTagAsync(Guid id);
        Task<IEnumerable<TagDto>> GetAllTags();
        Task<TagDto?> GetTagByIdAsync(Guid id);
        Task UpdateTagAsync(Guid id, TagUpdateDto Tag);
    }
}
