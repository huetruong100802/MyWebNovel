namespace MyWebNovel.Application.Pagination
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json.Serialization;

    public record PaginationParams(int PageNumber = 1, int PageSize = 10) : IParsable<PaginationParams>
    {
        [JsonIgnore]
        public int Skip => (PageNumber - 1) * PageSize;

        /// <summary>
        /// Parses a string into a PaginationParams object.
        /// </summary>
        /// <param name="s">Input string in the format "PageNumber,PageSize".</param>
        /// <param name="provider">An optional format provider.</param>
        /// <returns>A PaginationParams object.</returns>
        public static PaginationParams Parse(string s, IFormatProvider? provider)
        {
            if (string.IsNullOrWhiteSpace(s))
                return new PaginationParams(); // Use default values

            var parts = s.Split(',');
            if (parts.Length != 2 ||
                !int.TryParse(parts[0], out var pageNumber) ||
                !int.TryParse(parts[1], out var pageSize))
            {
                throw new FormatException($"Invalid format. Expected 'PageNumber,PageSize', but got: {s}");
            }

            return new PaginationParams(pageNumber, pageSize);
        }

        /// <summary>
        /// Tries to parse a string into a PaginationParams object.
        /// </summary>
        /// <param name="s">Input string in the format "PageNumber,PageSize".</param>
        /// <param name="provider">An optional format provider.</param>
        /// <param name="result">The resulting PaginationParams object, or null on failure.</param>
        /// <returns>True if parsing is successful; otherwise, false.</returns>
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out PaginationParams result)
        {
            result = null;

            if (string.IsNullOrWhiteSpace(s))
            {
                result = new PaginationParams(); // Use default values
                return true;
            }

            var parts = s.Split(',');
            if (parts.Length != 2 ||
                !int.TryParse(parts[0], out var pageNumber) ||
                !int.TryParse(parts[1], out var pageSize))
            {
                return false;
            }

            result = new PaginationParams(pageNumber, pageSize);
            return true;
        }
    }


}
