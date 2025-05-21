namespace MyWebNovel.Application.DTOs.NovelTag
{
    public record TagCreateDto(string Name, Domain.Entities.Novels.NovelTag.TagType Type, bool IsActive);
}
