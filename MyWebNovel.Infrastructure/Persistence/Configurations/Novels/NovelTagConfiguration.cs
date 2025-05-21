using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebNovel.Domain.Entities.Novels;

namespace MyWebNovel.Infrastructure.Persistence.Configurations.Novels
{
    public class NovelTagConfiguration : EntityBaseConfiguration<NovelTag>
    {
        public override void Configure(EntityTypeBuilder<NovelTag> builder)
        {
            // Primary Key Configuration
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                   .ValueGeneratedOnAdd();

            // Mandatory Fields
            builder.Property(e => e.Name)
                   .HasMaxLength(50)
                   .IsRequired();

            // Index Configuration
            builder.HasIndex(e => e.Name);
        }
    }
}
