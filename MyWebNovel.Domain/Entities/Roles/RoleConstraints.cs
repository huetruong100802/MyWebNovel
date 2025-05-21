namespace MyWebNovel.Domain.Entities.Roles
{
    public class RoleConstraints
    {
        public class Role
        {
            public const int MaxRoleNameLength = 16;
            public const int MinRoleNameLength = 3;
        }
    }
}
