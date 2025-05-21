using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebNovel.Domain.Entities.Roles;

namespace MyWebNovel.Infrastructure.Persistence.Configurations.Roles
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Name).HasMaxLength(50).IsRequired();

            // Optional: Add indexes for better query performance
            builder.HasIndex(e => e.Name);

            // Configure the one-to-many relationship with Permission
            builder.OwnsMany(r => r.Permissions, permissions =>
            {
                permissions.ToTable("RolePermissions"); // Join table for permissions
                permissions.WithOwner().HasForeignKey("RoleId");
                permissions.Property(p => p.ClassPermission).IsRequired().HasMaxLength(50);
                permissions.Property(p => p.PermissionLevel).IsRequired();
                permissions.HasKey("RoleId", "ClassPermission"); // Composite key to avoid duplication
            });

            builder.Navigation(r => r.Permissions).AutoInclude();
        }
    }
}
