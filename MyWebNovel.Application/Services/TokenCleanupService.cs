using Microsoft.Extensions.Logging;
using MyWebNovel.Application.Interfaces;

namespace MyWebNovel.Application.Services
{
    public class TokenCleanupService(IUnitOfWork unitOfWork, ILogger<TokenCleanupService> logger)
    {
        private readonly TimeSpan _cleanupThreshold = TimeSpan.FromDays(30);

        public async Task CleanupExpiredTokensAsync()
        {
            logger.LogInformation("Starting token cleanup process.");

            var cutoffDate = DateTime.UtcNow - _cleanupThreshold;
            var expiredTokens = await unitOfWork.RefreshTokens.GetExpiredTokensAsync(cutoffDate);

            if (expiredTokens.Any())
            {
                foreach (var token in expiredTokens)
                {
                    unitOfWork.RefreshTokens.DeleteToken(token);
                }
                await unitOfWork.SaveChangesAsync();

                logger.LogInformation("{Count} expired tokens cleaned up.", expiredTokens.Count());
            }
            else
            {
                logger.LogInformation("No expired tokens found for cleanup.");
            }
        }
    }
}
