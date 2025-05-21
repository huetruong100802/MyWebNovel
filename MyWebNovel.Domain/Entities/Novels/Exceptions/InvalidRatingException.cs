using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Domain.Entities.Novels.Exceptions
{
    public class InvalidRatingException(decimal rating)
        : DomainException($"Invalid Rating: '{rating}'",
            new Exception($"Rating must be between {NovelConstraints.Novel.MinRating} and {NovelConstraints.Novel.MaxRating}."))
    {
    }
}
