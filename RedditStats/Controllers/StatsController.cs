using Microsoft.AspNetCore.Mvc;
using RedditStats.Services;
using RedditStats.Models;

namespace RedditStats.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatsController(ICacheService cacheService, ILogger<StatsController> logger) : ControllerBase
{

    [HttpGet("topposts")]
    public async Task<IActionResult> GetTopPosts()
    {
        try
        {
            var topPosts = await cacheService.GetAsync<object>("topPosts");
            if (topPosts == null)
            {
                logger.LogWarning("Top posts not found");
                return NotFound("Top posts not found");
            }
            return Ok(topPosts);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred - top posts");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("topusers")]
    public async Task<IActionResult> GetTopUsers()
    {
        try
        {
            var topUsers = await cacheService.GetAsync<object>("topUsers");
            if (topUsers == null)
            {
                logger.LogWarning("Top users not found");
                return NotFound("Top users not found");
            }
            return Ok(topUsers);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred - top users");
            return StatusCode(500, "Internal server error");
        }
    }
}