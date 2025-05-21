using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Domain.Entities.Roles.Exceptions
{
    public class InvalidRoleNameException(string roleName)
        : DomainException($"Invalid Role Name: {roleName}.",
            new Exception($"Role name must be at between {RoleConstraints.Role.MinRoleNameLength} and {RoleConstraints.Role.MaxRoleNameLength} characters."))
    {
    }
}
