namespace MyWebNovel.Application.Pagination
{
    public static class PaginationExtensions
    {
        public static PaginatedResult<T> ToPaginatedResult<T>(this IEnumerable<T> items, PaginationParams paginationParams)
        {
            // Pagination
            var totalCount = items.Count();

            items = [.. items
                 .Skip(paginationParams.Skip)
                 .Take(paginationParams.PageSize)];

            return new PaginatedResult<T>(items, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
        }
    }
}
