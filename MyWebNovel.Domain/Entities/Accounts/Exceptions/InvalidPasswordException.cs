using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Domain.Entities.Accounts.Exceptions
{
    public class InvalidPasswordException(string password)
        : DomainException($"The provided password '{password}' does not meet the required criteria.",
            new Exception($"Password must be at between {AccountConstraint.Account.PasswordMaxLength} and {AccountConstraint.Account.PasswordMaxLength} characters long, " +
                $"contain at least one letter, one digit, one uppercase letter, and one symbol."))
    {
    }
}
