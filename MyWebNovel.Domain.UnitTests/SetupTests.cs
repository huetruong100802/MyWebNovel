using Moq;
using MyWebNovel.Domain.Entities.Shared.TimeProviders;

namespace MyWebNovel.Domain.UnitTests
{
    public class SetupTests
    {
        public readonly DateTimeOffset _currentTime;
        public readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
        public SetupTests()
        {
            _currentTime = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _dateTimeProviderMock.Setup(tp => tp.UtcNow).Returns(_currentTime);
        }
    }
}
