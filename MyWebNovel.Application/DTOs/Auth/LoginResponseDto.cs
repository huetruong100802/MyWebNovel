namespace MyWebNovel.Application.DTOs.Auth
{
    public record LoginResponseDto(
        string AccessToken, // The JWT token for authenticating subsequent requests
        DateTime ExpiresAt, // The expiration time of the access token
        string RefreshToken // The token used to refresh the access token
    );

}
