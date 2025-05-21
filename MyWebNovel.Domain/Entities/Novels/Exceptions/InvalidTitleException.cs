using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Domain.Entities.Novels.Exceptions
{
    public class InvalidTitleException(string title)
        : DomainException($"Invalid Title: '{title}'",
            new Exception($"Title must be between {NovelConstraints.Novel.TitleMinLength} and {NovelConstraints.Novel.TitleMaxLength} characters."))
    {
    }
}
