using Moq;
using RedditStats.Models;
using RedditStats.Services;
using Microsoft.Extensions.Logging;

namespace RedditStats.Tests;

public class StatsServiceTests
{
    [Fact]
    public async Task CalculateAndCacheStatsAsync_ShouldCalculateCorrectly()
    {
        // Arrange
        var cacheServiceMock = new Mock<ICacheService>();
        var redditServiceMock = new Mock<IRedditService>();

        var posts = new List<RedditPost>
        {
            new RedditPost { Title = "Post 1", Author = "User1", Ups = 10 },
            new RedditPost { Title = "Post 2", Author = "User2", Ups = 20 },
            new RedditPost { Title = "Post 3", Author = "User1", Ups = 15 },
            new RedditPost { Title = "Post 4", Author = "User3", Ups = 50 },
            new RedditPost { Title = "Post 5", Author = "User5", Ups = 30 }
        };

        redditServiceMock.Setup(r => r.GetPostsAsync(It.IsAny<string>())).ReturnsAsync(posts);

        var statsService = new StatsService(cacheServiceMock.Object, redditServiceMock.Object, Mock.Of<ILogger<StatsService>>());

        // Act
        await statsService.CalculateAndCacheStatsAsync("all");

        // Assert
        cacheServiceMock.Verify(c => c.SetAsync("topPosts", It.IsAny<List<RedditPost>>(), null), Times.Once);
        cacheServiceMock.Verify(c => c.SetAsync("topUsers", It.IsAny<List<UserPostStats>>(), null), Times.Once);
    }
}