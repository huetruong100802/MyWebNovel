using MyWebNovel.Domain.Entities.Novels.Exceptions;

namespace MyWebNovel.Domain.Entities.Novels
{
    public sealed record Title
    {
        public string Value { get; init; }

        public Title(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText)
                || plainText.Length < NovelConstraints.Novel.TitleMinLength
                || plainText.Length > NovelConstraints.Novel.TitleMaxLength)
            {
                throw new InvalidTitleException(plainText);
            }
            Value = plainText.Trim();
        }

        public static implicit operator string(Title plainText) => plainText.Value;
        public static implicit operator Title(string plainText) => new(plainText);
    }
}
