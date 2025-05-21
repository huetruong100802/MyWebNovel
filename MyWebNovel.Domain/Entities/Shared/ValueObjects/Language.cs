namespace MyWebNovel.Domain.Entities.Shared.ValueObjects
{
    using System;

    public sealed record Language
    {
        public enum ValidLanguages
        {
            en,
            vi
        }

        public ValidLanguages Code { get; init; }

        private Language(ValidLanguages code)
        {
            Code = code;
        }

        public static Language Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Language cannot be empty.", nameof(value));
            }

            if (!Enum.TryParse(value, true, out ValidLanguages parsedValue))
            {
                var validValues = string.Join(", ", Enum.GetNames<ValidLanguages>());
                throw new ArgumentException(
                    $"Invalid language: {value}. Valid options are: {validValues}.", nameof(value));
            }

            return parsedValue switch
            {
                ValidLanguages.en => English,
                ValidLanguages.vi => Vietnamese,
                _ => throw new ArgumentException($"Invalid language: {value}.", nameof(value))
            };
        }

        public static Language English => new(ValidLanguages.en);
        public static Language Vietnamese => new(ValidLanguages.vi);

        public static implicit operator string(Language language) => language.Code.ToString();
        public static implicit operator Language(string value) => Create(value);
    }

}
