using MyWebNovel.Domain.Entities.Shared;
using MyWebNovel.Domain.Entities.Shared.ValueObjects;

namespace MyWebNovel.Domain.Entities.Novels
{
    public sealed class Novel : EntityBase
    {
        public Title Title { get; private set; }
        private readonly List<NovelTag> _tags;
        public IReadOnlyList<NovelTag> Tags => _tags.AsReadOnly();
        public Synopsis Synopsis { get; private set; }
        public Language OriginalLanguage { get; private set; }
        public Guid AuthorId { get; private set; }
        public PublicationStatus PublicationStatus { get; private set; }
        public DateTimeOffset? PublicationDate { get; private set; }
        public Rating Rating { get; private set; }
        public int ViewCount { get; private set; }
        public ContentRating ContentRating { get; private set; }

        private Novel()
        {
            Title = string.Empty;
            _tags = new List<NovelTag>();
            Synopsis = string.Empty;
            PublicationStatus = PublicationStatus.Draft;
            OriginalLanguage = Language.English;
            Rating = Rating.Default;
            ContentRating = ContentRating.General;
        }

        private Novel(
            string title,
            string synopsis,
            Language language,
            Guid authorId,
            PublicationStatus publicationStatus,
            DateTimeOffset? publicationDate,
            ContentRating contentRating)
        {
            Title = title;
            _tags = [];
            Synopsis = synopsis;
            OriginalLanguage = language;
            AuthorId = authorId;
            PublicationStatus = publicationStatus;
            PublicationDate = publicationDate;
            Rating = Rating.Default;
            ContentRating = contentRating;
        }

        public static Novel Create(
            string title,
            string synopsis,
            Language language,
            Guid authorId,
            PublicationStatus publicationStatus,
            DateTimeOffset? publicationDate,
            ContentRating contentRating)
        {
            if (authorId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(AuthorId));
            return new Novel(title, synopsis, language, authorId, publicationStatus, publicationDate, contentRating);
        }

        /// <summary>
        /// Adds a Tag to the novel.
        /// </summary>
        /// <param name="tag"></param>
        public void AddTag(NovelTag tag)
        {
            ArgumentNullException.ThrowIfNull(tag, nameof(tag));
            if (!_tags.Any(g => g.Id == tag.Id))
            {
                _tags.Add(tag);
                UpdateLastModified();
            }
        }

        /// <summary>
        /// Adds multiple Tags to the novel.
        /// </summary>
        /// <param name="tags"></param>
        public void AddTags(IEnumerable<NovelTag> tags)
        {
            if (tags == null || !tags.Any())
                throw new ArgumentNullException(nameof(tags));
            foreach (var tag in tags)
            {
                AddTag(tag);
            }
        }

        /// <summary>
        /// Removes a Tag from the novel.
        /// </summary>
        /// <param name="tag"></param>
        public void RemoveTag(NovelTag tag)
        {
            ArgumentNullException.ThrowIfNull(tag, nameof(tag));
            if (_tags.Any(g => g.Id == tag.Id))
            {
                _tags.Remove(tag);
                UpdateLastModified();
            }
        }

        /// <summary>
        /// Updates the novel with the new information.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="synopsis"></param>
        /// <param name="language"></param>
        /// <param name="publicationStatus"></param>
        /// <param name="publicationDate"></param>
        public void Update(
            string title,
            string synopsis,
            Language language,
            PublicationStatus publicationStatus,
            DateTimeOffset? publicationDate,
            ContentRating contentRating)
        {
            Title = title;
            Synopsis = synopsis;
            OriginalLanguage = language;
            PublicationStatus = publicationStatus;
            PublicationDate = publicationDate;
            ContentRating = contentRating;

            UpdateLastModified();
        }
    }
}

