using RedditStats.Models;

namespace RedditStats.Services;

public interface IRedditService
{
    Task<List<RedditPost>> GetPostsAsync(string subreddit);
}
