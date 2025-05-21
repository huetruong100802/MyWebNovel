namespace MyWebNovel.Domain.Entities.Novels.Filters
{
    public record NovelFilter(
        string? Title,
        List<string>? Languages,
        List<string>? Tags,
        List<PublicationStatus>? PublicationStatuses,
        decimal? MinimumRating,
        List<ContentRating>? ContentRatings
    );
}
