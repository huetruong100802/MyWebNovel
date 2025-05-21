using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebNovel.Domain.Entities.Accounts;
using MyWebNovel.Domain.Entities.Roles;

namespace MyWebNovel.Infrastructure.Persistence.Configurations.Accounts
{
    public class AccountConfiguration : EntityBaseConfiguration<Account>
    {
        public override void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Username).HasMaxLength(AccountConstraint.Account.FullNameMaxLength)
                .IsRequired()
                .HasConversion(username => username.Value, value => value);
            builder.Property(e => e.Password).HasMaxLength(AccountConstraint.Account.PasswordMaxLength)
                .IsRequired()
                .HasConversion(password => password.Value, value => new Password(value));
            builder.Property(n => n.Email)
               .IsRequired()
               .HasConversion(
                   email => email.Value,
                   value => value
               );
            builder.Property(x => x.RoleId).IsRequired();

            // Indexes
            builder.HasIndex(a => a.Email)
                .IsUnique();

            builder.HasIndex(a => a.Username)
                .IsUnique();

            // Relationships
            builder.HasOne<Role>()
                .WithMany()
                .HasForeignKey(a => a.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
