###### Run redis container
docker run --name my-redis -p 6379:6379 -d redis

###### Replace API credential in appsetting.json file
ClientId: Reddit App ID
ClientSecret: Reddit App Secret
UserAgent: Reddit API user agent string e.g. "windows:<App Name>:<App Version> (by /u/your-user-name)"
