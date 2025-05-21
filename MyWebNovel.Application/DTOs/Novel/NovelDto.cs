using MyWebNovel.Domain.Entities.Novels;

namespace MyWebNovel.Application.DTOs.Novel
{
    public record NovelDto(
        Guid Id,
        string Title,
        string Synopsis,
        string Language,
        Guid AuthorId,
        PublicationStatus PublicationStatus,
        DateTimeOffset? PublicationDate,
        string? Rating,
        List<string> Tags,
        string ContentRating
    );

}
