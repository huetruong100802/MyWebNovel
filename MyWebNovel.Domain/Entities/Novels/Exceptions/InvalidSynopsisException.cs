using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Domain.Entities.Novels.Exceptions
{
    public class InvalidSynopsisException(string synopsis)
        : DomainException($"Invalid Synopsis: '{synopsis}'",
            new Exception($"Synopsis must be between {NovelConstraints.Novel.SynopsisMinLength} and {NovelConstraints.Novel.SynopsisMaxLength} characters."))
    {
    }
}
