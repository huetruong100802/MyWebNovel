using MyWebNovel.Domain.Entities.Novels;

namespace MyWebNovel.Application.DTOs.Novel
{
    public record NovelUpdateDto(
        string? Title,
        string? Synopsis,
        string? Language,
        DateTimeOffset? PublicationDate,
        PublicationStatus? PublicationStatus,
        ContentRating? ContentRating
        );

}
