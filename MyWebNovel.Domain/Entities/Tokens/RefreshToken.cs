using MyWebNovel.Domain.Entities.Shared.TimeProviders;
using MyWebNovel.Domain.Entities.Tokens.Exceptions;

namespace MyWebNovel.Domain.Entities.Tokens
{
    public sealed class RefreshToken
    {
        public Guid Id { get; private init; }
        public DateTimeOffset CreatedAt { get; private set; }
        public Guid UserId { get; private set; }
        public string Token { get; private set; }
        public DateTimeOffset Expiration { get; private set; }
        public DateTimeOffset? RevokedAt { get; private set; }

        private RefreshToken()
        {
            Token = string.Empty;
        }
        private RefreshToken(Guid userId, string token, DateTimeOffset expiration, DateTimeOffset createdAt)
        {
            UserId = userId;
            Token = token;
            Expiration = expiration;
            CreatedAt = createdAt;
            RevokedAt = null;
        }

        /// <summary>
        /// Creates a new refresh token.
        /// </summary>
        /// <param name="userId">The user ID associated with the token.</param>
        /// <param name="token">The token string.</param>
        /// <param name="expiration">The expiration timestamp.</param>
        /// <param name="createdAt">The created timestamp.</param>
        /// <returns>A new instance of RefreshToken.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static RefreshToken Create(Guid userId, string token, DateTimeOffset expiration, DateTimeOffset createdAt)
        {
            if (userId.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(userId));
            ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));
            if (expiration <= createdAt)
                throw new TokenExpirationException(expiration, createdAt);

            return new RefreshToken(userId, token, expiration, createdAt);
        }

        /// <summary>
        /// Checks if the token is expired.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>True if the token is expired; otherwise false.</returns>
        public bool IsExpired() => DateTimeProvider.Instance.UtcNow >= Expiration;

        /// <summary>
        /// Checks if the token is revoked.
        /// </summary>
        /// <returns>True if the token is revoked; otherwise false.</returns>
        public bool IsRevoked() => RevokedAt.HasValue;

        /// <summary>
        /// Revokes the token.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Revoke()
        {
            if (IsRevoked())
                throw new InvalidOperationException("Token is already revoked.");

            RevokedAt = Shared.TimeProviders.DateTimeProvider.Instance.UtcNow;
        }
    }
}
