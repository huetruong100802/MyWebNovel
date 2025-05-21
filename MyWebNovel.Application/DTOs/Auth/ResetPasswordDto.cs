namespace MyWebNovel.Application.DTOs.Auth
{
    public record ResetPasswordDto(string Email, string ResetToken, string NewPassword);
}
