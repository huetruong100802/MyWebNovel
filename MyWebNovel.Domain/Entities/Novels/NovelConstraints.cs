namespace MyWebNovel.Domain.Entities.Novels
{
    public class NovelConstraints
    {
        public class Novel
        {
            public const int TitleMinLength = 1;
            public const int TitleMaxLength = 250;
            public const int SynopsisMinLength = 250;
            public const int SynopsisMaxLength = 750;
            public const decimal MinRating = 0M;
            public const decimal MaxRating = 10;
        }
    }
}
