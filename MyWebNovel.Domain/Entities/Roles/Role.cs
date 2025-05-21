using MyWebNovel.Domain.Entities.Enums;
using MyWebNovel.Domain.Entities.Roles.Exceptions;

namespace MyWebNovel.Domain.Entities.Roles
{
    public sealed class Role
    {
        public int Id { get; private init; }
        public string Name { get; private set; }

        private readonly List<Permission> _permissions = new();
        public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();

        private Role()
        {
            Name = string.Empty;
        }

        private Role(int id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Create a new role.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns>A new Role instance.</returns>
        public static Role Create(int id, string name)
        {
            if (id < 1) throw new InvalidRoleIdException(id.ToString());
            if (string.IsNullOrWhiteSpace(name)
                || name.Length > RoleConstraints.Role.MaxRoleNameLength
                || name.Length < RoleConstraints.Role.MinRoleNameLength
                || !name.All(char.IsLetter)
                )
                throw new InvalidRoleNameException(name);
            return new Role(id, name.Trim());
        }

        /// <summary>
        /// Add a permission to the role.
        /// </summary>
        /// <param name="permission"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void AddPermission(Permission permission)
        {
            if (_permissions.Any(x => x.ClassPermission == permission.ClassPermission))
            {
                throw new InvalidOperationException("Permission already exists.");
            }
            _permissions.Add(permission);
        }

        /// <summary>
        /// Add multiple permissions to the role.
        /// </summary>
        /// <param name="permissions"></param>
        public void AddPermissions(IEnumerable<Permission> permissions)
        {
            foreach (var permission in permissions)
            {
                AddPermission(permission);
            }
        }

        /// <summary>
        /// Remove a permission from the role.
        /// </summary>
        /// <param name="permission"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void RemovePermission(Permission permission)
        {
            if (!_permissions.Contains(permission))
            {
                throw new InvalidOperationException("Permission does not exist.");
            }
            _permissions.Remove(permission);
        }

        /// <summary>
        /// Check if the role has the required permission.
        /// </summary>
        /// <param name="classPermission"></param>
        /// <param name="requiredLevel"></param>
        /// <returns>True if the role has the permission; otherwise, false.</returns>
        public bool HasPermission(string classPermission, PermissionLevelEnum requiredLevel)
        {
            var permission = _permissions.FirstOrDefault(p => p.ClassPermission == classPermission);
            if (permission == null) return false;

            return permission.PermissionLevel >= requiredLevel;
        }
    }
}
