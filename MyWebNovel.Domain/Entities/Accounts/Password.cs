using MyWebNovel.Domain.Entities.Accounts.Exceptions;

namespace MyWebNovel.Domain.Entities.Accounts
{
    public sealed record Password
    {
        public string Value { get; init; }
        public Password(string value)
        {
            Value = value;
        }
        public static Password CreateHashed(string plainText)
        {
            plainText = plainText.Trim();
            if (plainText.Length < AccountConstraint.Account.PasswordMinLength
                || plainText.Length > AccountConstraint.Account.PasswordMaxLength
                || !plainText.Any(char.IsLetterOrDigit)
                || !plainText.Any(char.IsUpper)
                || !(plainText.Any(char.IsSymbol)
                || plainText.Any(char.IsPunctuation))
                || plainText.Contains(' '))
            {
                throw new InvalidPasswordException(plainText);
            }
            return new Password(BCrypt.Net.BCrypt.HashPassword(plainText));
        }
        public bool Verify(string plainTextPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, Value);
        }

        public static explicit operator string(Password password)
            => password.Value;
        public static implicit operator Password(string plainText) => CreateHashed(plainText);
    }
}
