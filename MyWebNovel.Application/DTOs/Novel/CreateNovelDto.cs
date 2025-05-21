namespace MyWebNovel.Application.DTOs.Novel
{
    public record CreateNovelDto(
        string Title,
        string Synopsis,
        string Language, // Assuming ISO 639-1 language codes.
        string ContentRating, // e.g., "General", "Mature".
        IEnumerable<string> Tags, // Optional list of tags.
        DateTimeOffset? PublicationDate
    )
    {
        public Guid AuthorId { get; init; } //set this independently of the constructor
    }


}
