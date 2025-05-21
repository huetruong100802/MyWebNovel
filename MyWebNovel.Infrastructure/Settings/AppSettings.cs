namespace MyWebNovel.Infrastructure.Settings
{
    public record AppSettings
    {
        public required ConnectionStrings ConnectionStrings { get; init; }
        public required Jwt Jwt { get; init; }
        public required Smtp Smtp { get; init; }
    }
}
