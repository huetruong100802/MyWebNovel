namespace MyWebNovel.Domain.Entities.Shared.TimeProviders
{
    public static class DateTimeProvider
    {
        public static IDateTimeProvider Instance { get; set; } = new DefaultTimeProvider();
        private class DefaultTimeProvider : IDateTimeProvider
        {
            public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
        }
    }
}
