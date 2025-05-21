namespace MyWebNovel.Infrastructure.Settings
{
    public record Smtp(string Host, int Port, string Username, string Password, bool UseSsl, string FromAddress, string FromName);
}
