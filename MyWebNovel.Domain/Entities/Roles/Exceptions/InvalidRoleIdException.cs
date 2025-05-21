using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Domain.Entities.Roles.Exceptions
{
    public class InvalidRoleIdException(string roleId)
        : DomainException($"Invalid Role Id: {roleId}.",
            new Exception("Role Id can't be negative."))
    {
    }
}
