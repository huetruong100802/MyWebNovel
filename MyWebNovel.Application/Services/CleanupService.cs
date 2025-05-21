using Microsoft.Extensions.Logging;
using MyWebNovel.Application.Interfaces;
using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Application.Services
{
    public class CleanupService<T>(IUnitOfWork unitOfWork, ILogger<CleanupService<T>> logger) where T : EntityBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<CleanupService<T>> _logger = logger;
        private readonly TimeSpan _cleanupThreshold = TimeSpan.FromDays(30);

        public async Task CleanupAsync()
        {
            _logger.LogInformation("Starting cleanup for {EntityType} entities.", typeof(T).Name);

            var cutoffDate = DateTimeOffset.UtcNow - _cleanupThreshold;
            var repository = _unitOfWork.GetRepository<T>();
            var expiredEntities = await repository.GetSoftDeletedBeforeAsync(cutoffDate);

            if (expiredEntities.Any())
            {
                foreach (var entity in expiredEntities)
                {
                    repository.HardDelete(entity);
                }

                await _unitOfWork.SaveChangesAsync(); // Commit transaction through Unit of Work

                _logger.LogInformation("{Count} expired {EntityType} entities cleaned up.", expiredEntities.Count(), typeof(T).Name);
            }
            else
            {
                _logger.LogInformation("No expired {EntityType} entities found for cleanup.", typeof(T).Name);
            }
        }
    }


}
