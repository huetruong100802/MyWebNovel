using MyWebNovel.Domain.Entities.Tokens;

namespace MyWebNovel.Application.Interfaces
{
    public interface IJwtTokenService
    {
        AccessToken GenerateAccessToken(Guid userId, string username, int roleId);
        Task<AccessToken> GenerateAccessToken(Guid userId);
        Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId);
        Task<RefreshToken?> ValidateRefreshTokenAsync(string token);
        Task<bool> RevokeRefreshTokenAsync(string token);
    }
}
