﻿namespace RedditStats.Services;

public interface ICacheService
{
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task<T> GetAsync<T>(string key);
}