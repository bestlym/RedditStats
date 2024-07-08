namespace RedditStats.Services;

public class RedditBackgroundService(IStatsService statsService, ILogger<RedditBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Background data loader running");

            try
            {
                await statsService.CalculateAndCacheStatsAsync("askreddit");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while loading data in the background");
            }
            
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);  // Run every 5 minutes
        }
    }
}
