using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Domain.Entities.Accounts.Exceptions
{
    public class InvalidEmailDomainException(string email)
        : DomainException($"Invalid Email Address: '{email}'.",
            new Exception("Invalid domain format."))
    {
    }
}
