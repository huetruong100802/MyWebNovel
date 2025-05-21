using MyWebNovel.Domain.Entities.Accounts.Exceptions;

namespace MyWebNovel.Domain.Entities.Accounts
{
    public sealed record Username
    {
        public string Value { get; init; }

        public Username(string username)
        {
            if (string.IsNullOrWhiteSpace(username)
                || username.Length < AccountConstraint.Account.FullNameMinLength
                || username.Length > AccountConstraint.Account.FullNameMaxLength)
            {
                throw new InvalidUsernameException(username);
            }
            Value = username.Trim();
        }

        public static implicit operator string(Username username) => username.Value;
        public static implicit operator Username(string username) => new(username);
    }
}
