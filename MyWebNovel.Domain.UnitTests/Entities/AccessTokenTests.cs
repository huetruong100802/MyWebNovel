using MyWebNovel.Domain.Entities.Shared.TimeProviders;
using MyWebNovel.Domain.Entities.Tokens;
using MyWebNovel.Domain.Entities.Tokens.Exceptions;

namespace MyWebNovel.Domain.UnitTests.Entities
{
    public class AccessTokenTests : SetupTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Throw_Null_Or_Empty_Exception_When_Access_Token_Is_Null_Or_Empty(string accessToken)
        {
            Assert.Throws<ArgumentException>(() => AccessToken.Create(accessToken, _currentTime.AddHours(1).UtcDateTime, _currentTime));
        }

        [Theory]
        [InlineData(-1)] // Expiration in the past
        [InlineData(0)]  // Expiration at the current time
        public void Should_Throw_Token_Expiration_Exception_When_Expiration_Is_In_The_Past(int hoursOffset)
        {
            // Arrange
            var expiration = _currentTime.AddHours(hoursOffset).UtcDateTime;

            // Act & Assert
            Assert.Throws<TokenExpirationException>(() => AccessToken.Create("access_token", expiration, _currentTime));
        }

        [Fact]
        public void Should_Create_Access_Token_When_Access_Token_And_Expiration_Are_Valid()
        {
            // Arrange
            var expiration = _currentTime.AddHours(1).UtcDateTime;

            // Act
            var accessToken = AccessToken.Create("access_token", expiration, _currentTime);

            // Assert
            Assert.Equal("access_token", accessToken.Value);
            Assert.Equal(expiration, accessToken.Expiration);
        }

        [Theory]
        [InlineData(1, true)] // time provider is 1 hour in the future
        [InlineData(0, true)] // time provider is the same as expiration
        [InlineData(-1, false)] // time provider is 1 hour in the past
        public void Should_Return_Result_When_Access_Token_Is_Expired(int hoursOffset, bool expected)
        {
            // Arrange
            var createdAt = _currentTime;
            var expiration = createdAt.AddHours(1).UtcDateTime;
            var accessToken = AccessToken.Create("access_token", expiration, createdAt);
            _dateTimeProviderMock.Setup(tp => tp.UtcNow).Returns(createdAt.AddHours(hoursOffset + 1));
            DateTimeProvider.Instance = _dateTimeProviderMock.Object;

            // Act
            var isExpired = accessToken.IsExpired();

            // Assert
            Assert.Equal(expected, isExpired);
        }
    }
}
