using MyWebNovel.Domain.Entities.Tokens;

namespace MyWebNovel.Domain.Entities.Tokens.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken);
        void DeleteToken(RefreshToken token);
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task<IEnumerable<RefreshToken>> GetExpiredTokensAsync(DateTime cutoffDate);
    }
}
