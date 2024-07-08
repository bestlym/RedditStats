namespace RedditStats.Models;

public class RedditPost
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Url { get; set; }
    public int Ups { get; set; }
    public int Num_comments { get; set; }
}

public class RedditResponse
{
    public RedditData Data { get; set; }
}

public class RedditData
{
    public List<RedditPostWrapper> Children { get; set; }
}

public class RedditPostWrapper
{
    public RedditPost Data { get; set; }
}

