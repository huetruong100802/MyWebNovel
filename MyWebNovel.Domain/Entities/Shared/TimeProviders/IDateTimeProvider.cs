namespace MyWebNovel.Domain.Entities.Shared.TimeProviders
{
    public interface IDateTimeProvider
    {
        public DateTimeOffset UtcNow { get; }
    }
}
