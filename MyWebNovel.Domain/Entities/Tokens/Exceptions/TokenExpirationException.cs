using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Domain.Entities.Tokens.Exceptions
{
    public class TokenExpirationException(DateTimeOffset expiration, DateTimeOffset createdAt)
        : DomainException($"Expiration {expiration} must be after {createdAt}.")
    {
    }
}
