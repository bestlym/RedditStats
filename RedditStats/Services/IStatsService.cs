namespace RedditStats.Services;

public interface IStatsService
{
    Task CalculateAndCacheStatsAsync(string subreddit);
}
