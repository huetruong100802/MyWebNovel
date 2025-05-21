using MyWebNovel.Domain.Entities.Novels.Exceptions;

namespace MyWebNovel.Domain.Entities.Novels
{
    public sealed record Rating
    {
        public decimal Value { get; private set; }
        public Rating(decimal value)
        {
            if (value < NovelConstraints.Novel.MinRating || value > NovelConstraints.Novel.MaxRating)
            {
                throw new InvalidRatingException(value);
            }
            Value = value;
        }

        public static string None => "N/A";

        public static Rating Default => new(NovelConstraints.Novel.MinRating);

        public static implicit operator string(Rating rating) => rating.Value == Default.Value ? Rating.None : rating.Value.ToString("N2");
        public static implicit operator decimal(Rating rating) => rating.Value;
        public static implicit operator Rating(decimal rating) => new(rating);
    }
}
