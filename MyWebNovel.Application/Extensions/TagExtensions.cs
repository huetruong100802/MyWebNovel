using MyWebNovel.Application.DTOs.NovelTag;
using MyWebNovel.Domain.Entities.Novels;

namespace MyWebNovel.Application.Extensions
{
    public static class TagExtensions
    {
        public static TagDto MapToDto(this NovelTag Tag)
        {
            return new TagDto(
                Tag.Id,
                Tag.Name
            );
        }
    }
}
