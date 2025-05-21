using MyWebNovel.Domain.Entities.Novels;
using MyWebNovel.Domain.Entities.Novels.Exceptions;
using MyWebNovel.Domain.Entities.Shared.TimeProviders;
using MyWebNovel.Domain.Entities.Shared.ValueObjects;

namespace MyWebNovel.Domain.UnitTests.Entities
{
    public class NovelTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("A")]
        public void Should_Throw_For_Create_Title_With_Invalid_Value(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                Assert.Throws<InvalidTitleException>(() => new Title(title));
                return;
            }
            Assert.Throws<InvalidTitleException>(() => new Title(new(title[0], NovelConstraints.Novel.TitleMaxLength + 1)));
        }

        [Fact]
        public void Should_Throw_For_Create_Synopsis_With_Invalid_Length()
        {
            Assert.Throws<InvalidSynopsisException>(() => new Synopsis(new('a', NovelConstraints.Novel.SynopsisMinLength - 1)));
            Assert.Throws<InvalidSynopsisException>(() => new Synopsis(new('a', NovelConstraints.Novel.SynopsisMaxLength + 1)));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(11)]
        public void Should_Throw_For_Create_Rating_With_Invalid_Value(decimal rating)
        {
            Assert.Throws<InvalidRatingException>(() => new Rating(rating));
        }

        [Fact]
        public void Should_Throw_For_Create_Novel_With_Invalid_AuthorId()
        {
            // Arrange
            var authorId = Guid.Empty;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Novel.Create("Title", "Synopsis", Language.English, authorId, PublicationStatus.Draft, null, ContentRating.General));
        }

        [Fact]
        public void Should_Create_Novel()
        {
            // Arrange
            var authorId = Guid.NewGuid();

            // Act
            var novel = Novel.Create("Title", "Synopsis", Language.English, authorId, PublicationStatus.Draft, null, ContentRating.General);

            // Assert
            Assert.Equal("Title", novel.Title);
            Assert.Equal("Synopsis", novel.Synopsis);
            Assert.Equal(Language.English, novel.OriginalLanguage);
            Assert.Equal(authorId, novel.AuthorId);
            Assert.Equal(PublicationStatus.Draft, novel.PublicationStatus);
            Assert.Null(novel.PublicationDate);
            Assert.Equal(Rating.Default, novel.Rating);
            Assert.Equal(ContentRating.General, novel.ContentRating);
        }

        [Fact]
        public void Should_Update_Novel()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var novel = Novel.Create("Title", "Synopsis", Language.English, authorId, PublicationStatus.Draft, null, ContentRating.General);

            // Act
            novel.Update("New Title", "New Synopsis", Language.Vietnamese, PublicationStatus.OnGoing, DateTimeProvider.Instance.UtcNow, ContentRating.Teen);

            // Assert
            Assert.Equal("New Title", novel.Title);
            Assert.Equal("New Synopsis", novel.Synopsis);
            Assert.Equal(Language.Vietnamese, novel.OriginalLanguage);
            Assert.Equal(PublicationStatus.OnGoing, novel.PublicationStatus);
            Assert.NotNull(novel.PublicationDate);
            Assert.Equal(Rating.Default, novel.Rating);
            Assert.Equal(ContentRating.Teen, novel.ContentRating);
        }

        [Fact]
        public void Should_Tag_Novel()
        {
            // Arrange
            var novel = Novel.Create("Title", "Synopsis", Language.English, Guid.NewGuid(), PublicationStatus.Draft, null, ContentRating.General);
            var tag = NovelTag.Create("Tag", NovelTag.TagType.Genre);

            // Act
            novel.AddTag(tag);

            // Assert
            Assert.Contains(tag, novel.Tags);
        }

        [Fact]
        public void Should_Throw_For_Tag_Novel_With_Invalid_Tag()
        {
            // Arrange
            var novel = Novel.Create("Title", "Synopsis", Language.English, Guid.NewGuid(), PublicationStatus.Draft, null, ContentRating.General);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => novel.AddTag(null!));
        }

        [Fact]
        public void Should_Tags_Novel()
        {
            // Arrange
            var novel = Novel.Create("Title", "Synopsis", Language.English, Guid.NewGuid(), PublicationStatus.Draft, null, ContentRating.General);
            List<NovelTag> tags = [NovelTag.Create("Tag", NovelTag.TagType.Genre), NovelTag.Create("Tag", NovelTag.TagType.Genre)];

            // Act
            novel.AddTags(tags);

            // Assert
            Assert.Equal(tags, novel.Tags);
        }

        [Fact]
        public void Should_Throws_For_Tags_Novel_With_Null_Or_Empty_Tags()
        {
            // Arrange
            var novel = Novel.Create("Title", "Synopsis", Language.English, Guid.NewGuid(), PublicationStatus.Draft, null, ContentRating.General);
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => novel.AddTags(null!));
            Assert.Throws<ArgumentNullException>(() => novel.AddTags([]));
        }

        [Fact]
        public void Should_Untag_Novel()
        {
            // Arrange
            var novel = Novel.Create("Title", "Synopsis", Language.English, Guid.NewGuid(), PublicationStatus.Draft, null, ContentRating.General);
            var tag = NovelTag.Create("Tag", NovelTag.TagType.Genre);

            // Act
            novel.AddTag(tag);
            novel.RemoveTag(tag);

            // Assert
            Assert.DoesNotContain(tag, novel.Tags);
        }

        [Fact]
        public void Should_Throw_For_Untag_Novel_With_Invalid_Tag()
        {
            // Arrange
            var novel = Novel.Create("Title", "Synopsis", Language.English, Guid.NewGuid(), PublicationStatus.Draft, null, ContentRating.General);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => novel.RemoveTag(null!));
        }
    }
}
