using MyWebNovel.Domain.Entities.Tokens.Exceptions;

namespace MyWebNovel.Domain.Entities.Tokens
{
    public sealed record AccessToken
    {
        public string Value { get; init; }
        public DateTimeOffset Expiration { get; init; }
        public DateTimeOffset CreatedAt { get; init; }

        private AccessToken(string accessToken, DateTimeOffset expiration, DateTimeOffset createdAt)
        {
            Value = accessToken;
            Expiration = expiration;
            CreatedAt = createdAt;
        }
        public static AccessToken Create(string accessToken, DateTime expiration, DateTimeOffset createdAt)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(accessToken, nameof(accessToken));
            if (expiration <= createdAt)
                throw new TokenExpirationException(expiration, createdAt);
            return new AccessToken(accessToken, expiration, createdAt);
        }

        /// <summary>
        /// Check if the token is expired
        /// </summary>
        /// <returns></returns>
        public bool IsExpired() => Expiration <= Shared.TimeProviders.DateTimeProvider.Instance.UtcNow;
    }

}
