using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Infrastructure.Persistence.Configurations
{
    public abstract class EntityBaseConfiguration<T> : IEntityTypeConfiguration<T> where T : EntityBase
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(m => m.Created)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(m => m.LastModified)
                   .IsRequired()
                   .ValueGeneratedOnUpdate();

            builder.Property(m => m.IsDeleted)
                   .IsRequired()
                   .HasDefaultValue(false);
            builder.HasQueryFilter(m => !m.IsDeleted);

            builder.Property(m => m.Deleted)
                   .IsRequired(false);
        }
    }
}
