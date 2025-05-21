using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Domain.Entities.Novels
{
    public sealed class NovelTag : EntityBase
    {
        public enum TagType
        {
            Format,
            Genre,
            Theme,
            Content
        }
        public string Name { get; private set; }
        public TagType Type { get; private set; }
        public bool IsActive { get; private set; }

        private NovelTag()
        {
            Name = string.Empty;
        }
        private NovelTag(string name, TagType tagType, bool isActive)
        {
            Name = name;
            Type = tagType;
            IsActive = isActive;
        }

        /// <summary>
        /// Creates a new Tag entity.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static NovelTag Create(string name, TagType type, bool isActive = true)
        {
            ValidateInputs(name);
            return new(name, type, isActive);
        }

        /// <summary>
        /// Updates the Tag entity with the new name.
        /// </summary>
        /// <param name="name"></param>
        public void Update(string name, TagType type, bool isActive)
        {
            ValidateInputs(name);
            Name = name;
            Type = type;
            IsActive = isActive;
            UpdateLastModified();
        }

        /// <summary>
        /// Validates the input for the Tag entity.
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="ArgumentException"></exception>
        private static void ValidateInputs(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
        }

        public override string ToString()
        {
            return Name;
        }
    }

}

