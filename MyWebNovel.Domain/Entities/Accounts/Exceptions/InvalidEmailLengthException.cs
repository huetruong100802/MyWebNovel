using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Domain.Entities.Accounts.Exceptions
{
    public class InvalidEmailLengthException(string email)
        : DomainException($"Invalid Email Address Length: '{email}'.",
            new Exception($"Email address max length is {AccountConstraint.Account.EmailMaxLength} characters"))
    {
    }
}
