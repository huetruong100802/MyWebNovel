using MyWebNovel.Domain.Entities.Enums;
using MyWebNovel.Domain.Entities.Roles;
using MyWebNovel.Domain.Entities.Roles.Exceptions;

namespace MyWebNovel.Domain.UnitTests.Entities
{
    public class RoleTests
    {
        [Fact]
        public void Should_Create_Role()
        {
            // Arrange
            int id = 1;
            string roleName = "Admin";
            // Act
            var role = Role.Create(id, roleName);
            // Assert
            Assert.Equal(roleName, role.Name);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Should_Throw_Invalid_Role_Id_Exception_For_Invalid_Role_Id(int id)
        {
            Assert.Throws<InvalidRoleIdException>(() =>
            {
                Role.Create(id, "Admin");
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData("AA")]
        [InlineData("AA123")]
        [InlineData("ABCDEFGHIJKLMNOPQ")]
        [InlineData("12345")]
        public void Should_Throw_Invalid_Role_Name_Exception_For_Invalid_Role_Name(string roleName)
        {
            Assert.Throws<InvalidRoleNameException>(() =>
            {
                Role.Create(1, roleName);
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Throw_Null_Or_Empty_Exception_For_Invalid_Permission(string permission)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Permission(permission, PermissionLevelEnum.View);
            });
        }

        [Fact]
        public void Should_Create_Permission()
        {
            // Arrange
            string classPermission = "Permission";
            PermissionLevelEnum permissionLevel = PermissionLevelEnum.View;
            // Act
            var permission = new Permission(classPermission, permissionLevel);
            // Assert
            Assert.Equal(classPermission, permission.ClassPermission);
            Assert.Equal(permissionLevel, permission.PermissionLevel);
        }

        [Fact]
        public void Should_Add_Permission_To_Role()
        {
            // Arrange
            var role = Role.Create(1, "Admin");
            var permission = new Permission("Permission", PermissionLevelEnum.View);
            // Act
            role.AddPermission(permission);
            // Assert
            Assert.Contains(permission, role.Permissions);
        }

        [Fact]
        public void Should_Add_Permissions_To_Role()
        {
            // Arrange
            var role = Role.Create(1, "Admin");
            var permission1 = new Permission("Permission", PermissionLevelEnum.View);
            var permission2 = new Permission("Account", PermissionLevelEnum.View);
            // Act
            role.AddPermissions([permission1, permission2]);
            // Assert
            Assert.Contains(permission1, role.Permissions);
            Assert.Contains(permission2, role.Permissions);
        }

        [Fact]
        public void Should_Throw_Invalid_Operation_Exception_When_Adding_Existing_Permission()
        {
            // Arrange
            var role = Role.Create(1, "Admin");
            var permission = new Permission("Permission", PermissionLevelEnum.View);
            var duplicatedPermission = new Permission("Permission", PermissionLevelEnum.FullAccess);
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                role.AddPermission(permission);
                role.AddPermission(duplicatedPermission);
            });
        }

        [Fact]
        public void Should_Remove_Permissions_To_Role()
        {
            // Arrange
            var role = Role.Create(1, "Admin");
            var permission1 = new Permission("Permission", PermissionLevelEnum.View);
            var permission2 = new Permission("Account", PermissionLevelEnum.View);
            // Act
            role.AddPermissions([permission1, permission2]);
            role.RemovePermission(permission1);
            // Assert
            Assert.DoesNotContain(permission1, role.Permissions);
            Assert.Contains(permission2, role.Permissions);
        }

        [Fact]
        public void Should_Throw_Exception_For_Removing_Non_Existent_Permission()
        {
            // Arrange
            var role = Role.Create(1, "Admin");
            var permission = new Permission("Permission", PermissionLevelEnum.View);
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                role.RemovePermission(permission);
            });
        }

        [Theory]
        [InlineData("Permission", PermissionLevelEnum.FullAccess, false)]
        [InlineData("Permission", PermissionLevelEnum.Modifed, true)]
        [InlineData("Permission", PermissionLevelEnum.View, true)]
        [InlineData("NonExistedPermision", PermissionLevelEnum.View, false)]
        public void Should_Check_If_Role_Has_Permission(string classPermission, PermissionLevelEnum requiredLevel, bool expected)
        {
            // Arrange
            var role = Role.Create(1, "Admin");
            var permission = new Permission("Permission", PermissionLevelEnum.Modifed);
            role.AddPermission(permission);
            // Act
            var hasPermission = role.HasPermission(classPermission, requiredLevel);
            // Assert
            Assert.Equal(expected, hasPermission);
        }
    }
}
