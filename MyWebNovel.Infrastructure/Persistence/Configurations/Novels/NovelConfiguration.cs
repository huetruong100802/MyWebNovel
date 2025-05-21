using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebNovel.Domain.Entities.Accounts;
using MyWebNovel.Domain.Entities.Novels;
using MyWebNovel.Domain.Entities.Shared.ValueObjects;

namespace MyWebNovel.Infrastructure.Persistence.Configurations.Novels
{
    public class NovelConfiguration : EntityBaseConfiguration<Novel>
    {
        public override void Configure(EntityTypeBuilder<Novel> builder)
        {
            // Primary Key Configuration
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                   .ValueGeneratedOnAdd();

            // Mandatory Fields
            builder.Property(e => e.Title)
                .HasMaxLength(NovelConstraints.Novel.TitleMaxLength)
                   .IsRequired()
                   .HasConversion(
                        title => title.Value,
                        value => value);

            builder.Property(n => n.OriginalLanguage)
                   .IsRequired()
                   .HasConversion(
                       lang => lang.Code.ToString(), // Store Language.Value in the database
                       value => Language.Create(value) // Convert back to Language object
                   );

            builder.Property(e => e.AuthorId)
                   .IsRequired();

            builder.Property(e => e.PublicationStatus)
                   .IsRequired();

            // Optional Fields
            builder.Property(e => e.Synopsis)
                    .HasMaxLength(NovelConstraints.Novel.SynopsisMaxLength)
                    .HasConversion(
                        synopsis => synopsis.Value,
                        value => value
                    );

            builder.Property(e => e.PublicationDate);

            builder.Property(e => e.Rating)
                    .HasConversion(
                        rating => rating.Value,
                        value => value
                    );

            builder.Property(e => e.ViewCount);

            // Index Configuration
            builder.HasIndex(e => e.Title);

            // Configure RoleId as a foreign key to Account entity
            builder.HasOne<Account>()
                   .WithMany()
                   .HasForeignKey(e => e.AuthorId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Configure Many-to-Many relationship between Tag and Novel entities
            builder.HasMany(e => e.Tags)
                   .WithMany()
                   .UsingEntity(j => j.ToTable("NovelTags"));

            builder.Navigation(r => r.Tags).AutoInclude();
        }
    }
}
