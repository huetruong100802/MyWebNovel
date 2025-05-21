using MyWebNovel.Domain.Entities.Shared.TimeProviders;

namespace MyWebNovel.Infrastructure.Shared
{
    public sealed class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
