using MyWebNovel.Domain.Entities.Tokens;
using MyWebNovel.Domain.Entities.Tokens.Exceptions;

namespace MyWebNovel.Domain.UnitTests.Entities
{
    public class RefreshTokenTests : SetupTests
    {
        [Fact]
        public void Should_Throw_Exception_For_Create_Refresh_Token_With_Empty_UserId()
        {
            // Arrange
            var userId = Guid.Empty;
            var token = Guid.NewGuid().ToString();
            var createdAt = _currentTime;
            var expiration = createdAt.AddHours(1);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => RefreshToken.Create(userId, token, expiration, createdAt));
        }

        [Fact]
        public void Should_Throw_Exception_For_Create_Refresh_Token_With_Empty_Token()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var token = "";
            var createdAt = _currentTime;
            var expiration = createdAt.AddHours(1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => RefreshToken.Create(userId, token, expiration, createdAt));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Throw_Exception_For_Create_Refresh_Token_With_Invalid_Expiration(int hoursOffset)
        {
            // Arrange
            var userId = Guid.NewGuid();
            var token = Guid.NewGuid().ToString();
            var createdAt = _currentTime;
            var expiration = createdAt.AddHours(hoursOffset);

            // Act & Assert
            Assert.Throws<TokenExpirationException>(() => RefreshToken.Create(userId, token, expiration, createdAt));
        }
    }
}
