namespace MyWebNovel.Domain.Entities.Accounts
{
    public class AccountConstraint
    {
        public class Account
        {
            public const int FullNameMinLength = 3;
            public const int FullNameMaxLength = 150;
            public const int PasswordMinLength = 8;
            public const int PasswordMaxLength = 16;
            public const int EmailMaxLength = 250;
            public const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        }
    }
}
