using MyWebNovel.Domain.Entities.Novels.Exceptions;

namespace MyWebNovel.Domain.Entities.Novels
{
    public sealed record Synopsis
    {
        public string Value { get; init; }

        public Synopsis(string plainText)
        {
            if (!string.IsNullOrEmpty(plainText)
                && (plainText.Length < NovelConstraints.Novel.SynopsisMinLength
                || plainText.Length > NovelConstraints.Novel.SynopsisMaxLength))
            {
                throw new InvalidSynopsisException(plainText);
            }
            Value = plainText.Trim();
        }

        public static implicit operator string(Synopsis plainText) => plainText.Value;
        public static implicit operator Synopsis(string plainText) => new(plainText);
    }
}
