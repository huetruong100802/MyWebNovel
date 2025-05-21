using MyWebNovel.Domain.Entities.Accounts.Exceptions;
using System.Text.RegularExpressions;
namespace MyWebNovel.Domain.Entities.Accounts
{
    public sealed record Email
    {
        public string Value { get; init; }
        private static readonly Regex EmailRegex = new(AccountConstraint.Account.EmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public Email(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !EmailRegex.IsMatch(email))
            {
                throw new InvalidEmailDomainException(email);
            }
            else if (email.Length > AccountConstraint.Account.EmailMaxLength)
            {
                throw new InvalidEmailLengthException(email);
            }
            else
            {
                Value = email.ToLowerInvariant();
            }
        }

        public static explicit operator string(Email email)
            => email.Value;
        public static implicit operator Email(string email) => new(email);
    }
}
