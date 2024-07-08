using StackExchange.Redis;
using Serilog;
using RedditStats.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Register services
builder.Services.AddControllers();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var redisConfiguration = ConfigurationOptions.Parse(configuration.GetSection("Redis:Configuration").Value);
    return ConnectionMultiplexer.Connect(redisConfiguration);
});
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IRedditService, RedditService>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddSingleton<IStatsService, StatsService>();
builder.Services.AddHostedService<RedditBackgroundService>();

// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RedditStatApp API", Version = "v1" });
});

var app = builder.Build();

// Configure middleware
// Configure Swagger
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RedditStatApp API v1"));
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();