using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Domain.UnitTests.Entities
{
    public class EntityBaseTests
    {
        private class TestEntity : EntityBase { }

        [Fact]
        public void UpdateLastModified_Should_Update_LastModified_To_CurrentUtcTime()
        {
            // Arrange
            var entity = new TestEntity();
            var originalLastModified = entity.LastModified;

            // Act
            entity.UpdateLastModified();

            // Assert
            Assert.NotEqual(originalLastModified, entity.LastModified);
            Assert.True(entity.LastModified > originalLastModified);
        }

        [Fact]
        public void SoftDelete_Should_Set_IsDeleted_To_True_And_Deleted_To_CurrentUtcTime()
        {
            // Arrange
            var entity = new TestEntity();
            var originalLastModified = entity.LastModified;

            // Act
            entity.SoftDelete();

            // Assert
            Assert.True(entity.IsDeleted);
            Assert.NotNull(entity.Deleted);
            Assert.True(Math.Abs((double)(entity.Deleted - entity.LastModified)?.TotalMilliseconds!) < 10);
            Assert.True(entity.LastModified > originalLastModified);
        }

        [Fact]
        public void SoftDelete_Should_Not_Update_If_Already_Deleted()
        {
            // Arrange
            var entity = new TestEntity();
            entity.SoftDelete();
            var deletedTimestamp = entity.Deleted;
            var lastModifiedTimestamp = entity.LastModified;

            // Act
            entity.SoftDelete();

            // Assert
            Assert.Equal(deletedTimestamp, entity.Deleted);
            Assert.Equal(lastModifiedTimestamp, entity.LastModified);
        }

        [Fact]
        public void Restore_Should_Set_IsDeleted_To_False_And_Reset_Deleted_To_Null()
        {
            // Arrange
            var entity = new TestEntity();
            entity.SoftDelete();
            var originalLastModified = entity.LastModified;

            // Act
            entity.Restore();

            // Assert
            Assert.False(entity.IsDeleted);
            Assert.Null(entity.Deleted);
            Assert.True(entity.LastModified > originalLastModified);
        }

        [Fact]
        public void Restore_Should_Not_Update_If_Not_Deleted()
        {
            // Arrange
            var entity = new TestEntity();
            var originalLastModified = entity.LastModified;

            // Act
            entity.Restore();

            // Assert
            Assert.False(entity.IsDeleted);
            Assert.Null(entity.Deleted);
            Assert.Equal(originalLastModified, entity.LastModified);
        }
    }
}

