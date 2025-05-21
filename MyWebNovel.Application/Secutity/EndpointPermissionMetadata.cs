using MyWebNovel.Domain.Entities.Enums;

namespace MyWebNovel.Application.Security
{
    public class EndpointPermissionMetadata(string classPermission, PermissionLevelEnum permissionLevel)
    {
        public string ClassPermission { get; set; } = classPermission ?? throw new ArgumentNullException(nameof(classPermission));
        public PermissionLevelEnum PermissionLevel { get; set; } = permissionLevel;
    }
}
