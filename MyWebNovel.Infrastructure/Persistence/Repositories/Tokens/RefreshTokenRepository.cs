using Microsoft.EntityFrameworkCore;
using MyWebNovel.Domain.Entities.Tokens;
using MyWebNovel.Domain.Entities.Tokens.Repositories;

namespace MyWebNovel.Infrastructure.Persistence.Repositories.Tokens
{
    public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
        }

        public void DeleteToken(RefreshToken token)
        {
            _context.RefreshTokens.Remove(token);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task<IEnumerable<RefreshToken>> GetExpiredTokensAsync(DateTime cutoffDate)
        {
            return [.. (await _context.RefreshTokens
                .AsNoTracking()
                .ToListAsync())
                .Where(x => x.IsExpired() || x.IsRevoked())];
        }
    }
}
