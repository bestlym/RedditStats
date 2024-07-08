using RedditStats.Models;
using System.Text.Json;

namespace RedditStats.Services;

public class RedditService(HttpClient httpClient, IConfiguration configuration, ILogger<RedditService> logger) : IRedditService
{
    public async Task<List<RedditPost>> GetPostsAsync(string subreddit)
    {
        try
        {
            var userAgent = configuration["Reddit:UserAgent"];
            logger.LogInformation("Using UserAgent: {UserAgent}", userAgent);
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://www.reddit.com/r/{subreddit}/hot.json");

            if (!httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", userAgent))
            {
                Console.Error.WriteLine("Failed to add the UA");
            }

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            JsonSerializerOptions jsonOption = new() { PropertyNameCaseInsensitive = true };
            var redditResponse = JsonSerializer.Deserialize<RedditResponse>(content, jsonOption);

            return redditResponse?.Data?.Children.Select(c => c.Data).ToList() ?? new List<RedditPost>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching Reddit posts");
            throw;
        }
    }
}
