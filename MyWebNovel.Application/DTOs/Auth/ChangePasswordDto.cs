namespace MyWebNovel.Application.DTOs.Auth
{
    public record ChangePasswordDto(Guid UserId, string OldPassword, string NewPassword);
}
