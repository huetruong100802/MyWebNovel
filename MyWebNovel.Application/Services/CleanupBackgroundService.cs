using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyWebNovel.Domain.Entities.Accounts;
using MyWebNovel.Domain.Entities.Novels;

namespace MyWebNovel.Application.Services
{
    public class CleanupBackgroundService(IServiceScopeFactory scopeFactory, ILogger<CleanupBackgroundService> logger, IConfiguration configuration) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly ILogger<CleanupBackgroundService> _logger = logger;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromHours(configuration.GetValue<int>("CleanupIntervalHours", 24));
        private readonly List<Type> _cleanupEntityTypes =
        [
            typeof(Account),
            typeof(Novel),
            typeof(NovelTag)
        ];

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Combined Cleanup Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Starting cleanup process at {Timestamp}.", DateTimeOffset.UtcNow);

                    using var scope = _scopeFactory.CreateScope();

                    // Cleanup tokens
                    var tokenCleanupService = scope.ServiceProvider.GetRequiredService<TokenCleanupService>();
                    await tokenCleanupService.CleanupExpiredTokensAsync();
                    _logger.LogInformation("Token cleanup process completed at {Timestamp}.", DateTimeOffset.UtcNow);

                    // Cleanup entities dynamically
                    foreach (var entityType in _cleanupEntityTypes)
                    {
                        _logger.LogInformation("Starting cleanup for entity type {EntityType} at {Timestamp}.", entityType.Name, DateTimeOffset.UtcNow);

                        var cleanupServiceType = typeof(CleanupService<>).MakeGenericType(entityType);
                        dynamic? cleanupService = scope.ServiceProvider.GetService(cleanupServiceType);

                        if (cleanupService != null)
                        {
                            await cleanupService.CleanupAsync();
                            _logger.LogInformation("Cleanup for entity type {EntityType} completed at {Timestamp}.", entityType.Name, DateTimeOffset.UtcNow);
                        }
                        else
                        {
                            _logger.LogWarning("Cleanup service for entity type {EntityType} not found.", entityType.Name);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during the combined cleanup process at {Timestamp}.", DateTimeOffset.UtcNow);
                }

                _logger.LogInformation("Waiting for the next cleanup cycle (interval: {Interval}).", _cleanupInterval);

                try
                {
                    // Wait for the next interval or handle cancellation
                    await Task.Delay(_cleanupInterval, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    // Handle cancellation gracefully during shutdown
                    _logger.LogInformation("Combined Cleanup Service task was canceled.");
                    break;
                }
            }

            _logger.LogInformation("Combined Cleanup Service is stopping.");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Cleanup background service is shutting down.");

            await base.StopAsync(cancellationToken);

            _logger.LogInformation("Cleanup background service shut down completed.");
        }
    }

}
