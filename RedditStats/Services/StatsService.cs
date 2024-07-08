using RedditStats.Models;

namespace RedditStats.Services;

public class StatsService(ICacheService cacheService, IRedditService redditService, ILogger<StatsService> logger) : IStatsService
{
    public async Task CalculateAndCacheStatsAsync(string subreddit)
    {
        try
        {
            var posts = await redditService.GetPostsAsync(subreddit);

            var topPosts = posts.OrderByDescending(p => p.Ups)
                .Take(20)
                .ToList();

            var topUsers = posts.GroupBy(p => p.Author)
                .OrderByDescending(g => g.Count())
                .Select(g => new UserPostStats { User = g.Key, PostCount = g.Count() })
                .ToList();

            await cacheService.SetAsync("topPosts", topPosts);
            await cacheService.SetAsync("topUsers", topUsers);
            logger.LogInformation($"Stats cached successfully: {DateTime.Now}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while caching stats");
            throw;
        }
    }
}