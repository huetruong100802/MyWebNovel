using MyWebNovel.Domain.Entities.Enums;

namespace MyWebNovel.Domain.Entities.Roles
{
    public sealed record Permission
    {
        public string ClassPermission { get; init; }
        public PermissionLevelEnum PermissionLevel { get; init; }

        public Permission(string classPermission, PermissionLevelEnum permissionLevel)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(classPermission, nameof(classPermission));
            ClassPermission = classPermission;
            PermissionLevel = permissionLevel;
        }
    }
}
