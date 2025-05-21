using MyWebNovel.Application.DTOs.Novel;
using MyWebNovel.Domain.Entities.Novels;
using MyWebNovel.Domain.Entities.Novels.Filters;

namespace MyWebNovel.Application.Extensions
{
    public static class NovelExtensions
    {
        public static NovelDto MapToDto(this Novel novel)
        {
            return new NovelDto(
                novel.Id,
                novel.Title,
                novel.Synopsis,
                novel.OriginalLanguage, // Assuming Language has a `Value` property for the language code.
                novel.AuthorId,
                novel.PublicationStatus,
                novel.PublicationDate,
                novel.Rating,
                [.. novel.Tags.Select(g => g.ToString())], // Assuming Tag has a `Name` property.
                Enum.GetName(novel.ContentRating)! // Assuming ContentRating is an enum.
            );
        }

        public static NovelFilter MapToNovelFilter(this NovelFilterDto novelFilterDto)
        {
            return new NovelFilter(
                novelFilterDto.Title,
                novelFilterDto.Languages,
                novelFilterDto.Tags,
                novelFilterDto.PublicationStatuses?
                    .Select(Enum.Parse<PublicationStatus>)
                    .ToList(),
                novelFilterDto.MinimumRating,
                novelFilterDto.ContentRatings?
                    .Select(Enum.Parse<ContentRating>)
                    .ToList()
            );
        }
    }
}
