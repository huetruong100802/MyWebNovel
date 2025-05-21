namespace MyWebNovel.Application.DTOs.Account
{
    public record AccountDto
    (
        Guid Id,
        string Username,
        string Email,
        string Role
     );
}
