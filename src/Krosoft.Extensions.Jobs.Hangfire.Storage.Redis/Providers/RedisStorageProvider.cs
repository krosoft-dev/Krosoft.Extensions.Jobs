#if NET9_0_OR_GREATER
using Hangfire;
using Hangfire.Redis.StackExchange;
using Krosoft.Extensions.Jobs.Hangfire.Interfaces;

namespace Krosoft.Extensions.Jobs.Hangfire.Storage.Redis.Providers;

/// <summary>
/// Provider de stockage Redis pour Hangfire
/// </summary>
public class RedisStorageProvider : IHangfireStorageProvider
{
    private readonly string _connectionString;
    private readonly RedisStorageOptions? _options;

    public RedisStorageProvider(string connectionString, RedisStorageOptions? options = null)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _options = options;
    }

    public void ConfigureStorage(IGlobalConfiguration configuration)
    {
        configuration.UseRedisStorage(_connectionString, _options ??
                                                         new RedisStorageOptions
                                                         {
                                                             Prefix = "hangfire:"
                                                         });
    }
}

#endif