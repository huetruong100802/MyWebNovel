using MyWebNovel.Domain.Entities.Shared.TimeProviders;

namespace MyWebNovel.Domain.Entities.Shared
{
    public abstract class EntityBase
    {
        public Guid Id { get; private init; } = Guid.NewGuid();
        public DateTimeOffset Created { get; private set; } = DateTimeProvider.Instance.UtcNow;
        public DateTimeOffset LastModified { get; private set; } = DateTimeProvider.Instance.UtcNow;
        public bool IsDeleted { get; private set; }
        public DateTimeOffset? Deleted { get; private set; }

        /// <summary>
        /// Updates the LastModified timestamp to the current UTC time.
        /// </summary>
        public void UpdateLastModified()
        {
            LastModified = DateTimeProvider.Instance.UtcNow;
        }

        /// <summary>
        /// Soft deletes the entity by setting the IsDeleted flag to true and setting the Deleted timestamp to the current UTC time.
        /// </summary>
        public void SoftDelete()
        {
            if (!IsDeleted)
            {
                IsDeleted = true;
                Deleted = DateTimeProvider.Instance.UtcNow;
                UpdateLastModified();
            }
        }

        /// <summary>
        /// Restores the entity by setting the IsDeleted flag to false and setting the Deleted timestamp to null.
        /// </summary>
        public void Restore()
        {
            if (IsDeleted)
            {
                IsDeleted = false;
                Deleted = null;
                UpdateLastModified();
            }
        }
    }
}
