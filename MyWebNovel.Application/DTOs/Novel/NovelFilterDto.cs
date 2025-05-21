namespace MyWebNovel.Application.DTOs.Novel
{
    using System.Diagnostics.CodeAnalysis;

    public record NovelFilterDto(
        string? Title,
        List<string> Languages,
        List<string> Tags,
        List<string> PublicationStatuses,
        decimal? MinimumRating,
        List<string> ContentRatings
    ) : IParsable<NovelFilterDto>
    {
        // Constructor for default initialization
        public NovelFilterDto()
            : this(null, [], [], [], null, [])
        {
        }

        /// <summary>
        /// Parses a string into a NovelFilterDto object.
        /// Expected format: "Title|Language1,Language2|Tag1,Tag2|PublicationStatus1,PublicationStatus2|MinimumRating|ContentRating1,ContentRating2".
        /// </summary>
        public static NovelFilterDto Parse(string s, IFormatProvider? provider)
        {
            if (string.IsNullOrWhiteSpace(s))
                throw new ArgumentException("Input string cannot be null or empty.", nameof(s));

            var parts = s.Split('|');
            if (parts.Length != 6)
                throw new FormatException($"Invalid format. Expected 6 parts separated by '|', but got: {s}");

            // Parsing individual parts
            var title = parts[0];
            var languages = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            var tags = parts[2].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            var publicationStatuses = parts[3].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            var minimumRating = decimal.TryParse(parts[4], out var rating) ? rating : decimal.Zero;
            var contentRatings = parts[5].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            return new NovelFilterDto(title, languages, tags, publicationStatuses, minimumRating, contentRatings);
        }

        /// <summary>
        /// Tries to parse a string into a NovelFilterDto object.
        /// </summary>
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out NovelFilterDto? result)
        {
            result = null;

            if (string.IsNullOrWhiteSpace(s))
                return false;

            var parts = s.Split('|');
            if (parts.Length != 6)
                return false;

            try
            {
                var title = parts[0];
                var languages = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                var tags = parts[2].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                var publicationStatuses = parts[3].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                var minimumRating = decimal.TryParse(parts[4], out var rating) ? rating : decimal.Zero;
                var contentRatings = parts[5].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

                result = new NovelFilterDto(title, languages, tags, publicationStatuses, minimumRating, contentRatings);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}
