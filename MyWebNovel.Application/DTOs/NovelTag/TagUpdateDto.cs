namespace MyWebNovel.Application.DTOs.NovelTag
{
    public record TagUpdateDto(string? Name, Domain.Entities.Novels.NovelTag.TagType? Type, bool? IsActive);
}
