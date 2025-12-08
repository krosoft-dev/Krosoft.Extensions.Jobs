#if NET9_0_OR_GREATER
using Hangfire;
using Hangfire.Redis.StackExchange;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Jobs.Hangfire.Storage.Redis.Providers;

namespace Krosoft.Extensions.Jobs.Hangfire.Storage.Redis.Extensions;

public static class KrosoftBackgroundJobServerOptionsExtensions
{
    public static BackgroundJobServerOptions UseRedisStorage(this KrosoftBackgroundJobServerOptions options,
                                                             string connectionString,
                                                             RedisStorageOptions? redisOptions = null)
    {
        options.StorageProvider = new RedisStorageProvider(connectionString, redisOptions);
        return options;
    }
}
#endif