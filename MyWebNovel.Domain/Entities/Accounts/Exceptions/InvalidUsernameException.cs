using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Domain.Entities.Accounts.Exceptions
{
    public class InvalidUsernameException(string username)
        : DomainException($"Invalid Username: '{username}'.",
            new Exception($"Username must be at between {MinimumUsernameLength} and {MaximumUsernameLength} characters."))
    {
        private static readonly int MinimumUsernameLength = AccountConstraint.Account.FullNameMinLength;
        private static readonly int MaximumUsernameLength = AccountConstraint.Account.FullNameMaxLength;
    }
}
