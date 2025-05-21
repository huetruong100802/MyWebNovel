using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebNovel.Domain.Entities.Tokens;

namespace MyWebNovel.Infrastructure.Persistence.Configurations.Tokens
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            // Primary Key
            builder.HasKey(rt => rt.Id);

            // Properties
            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(512); // Adjust the length as per your token size

            builder.Property(rt => rt.UserId)
                .IsRequired();

            builder.Property(rt => rt.Expiration)
                .IsRequired();

            builder.Property(rt => rt.CreatedAt)
                .IsRequired();

            builder.Property(rt => rt.RevokedAt)
                .IsRequired(false); // Nullable for tokens that haven’t been revoked

            // Indexes (Optional)
            builder.HasIndex(rt => rt.Token)
                .IsUnique(); // Prevent duplicate tokens
        }
    }

}
