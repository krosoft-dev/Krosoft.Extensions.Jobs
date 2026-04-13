using Hangfire;
#if NET9_0_OR_GREATER
using Hangfire.Redis.StackExchange;
#else
using Hangfire.Redis;
#endif
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Jobs.Hangfire.Storage.Redis.Stores;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Krosoft.Extensions.Jobs.Hangfire.Storage.Redis.Providers;

/// <summary>
/// Provider de stockage Redis pour Hangfire
/// </summary>
public class RedisStorageProvider : IHangfireStorageProvider
{
    private const string DefaultPrefix = "hangfire:";

    private readonly Lazy<IConnectionMultiplexer>? _connection;
    private readonly RedisStorageOptions? _options;

    public RedisStorageProvider(string? connectionString, RedisStorageOptions? options = null)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(connectionString), connectionString);

        _connection = new Lazy<IConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString!));
        _options = options;
    }

    private IConnectionMultiplexer Connection
    {
        get
        {
            if (_connection != null)
            {
                return _connection.Value;
            }

            throw new KrosoftTechnicalException("Connection non disponible !");
        }
    }

    private string Prefix => _options?.Prefix ?? DefaultPrefix;

    public void ConfigureStorage(IGlobalConfiguration configuration)
    {
        configuration.UseRedisStorage(Connection, _options ??
                                                  new RedisStorageOptions
                                                  {
                                                      Prefix = DefaultPrefix
                                                  });
    }

    public void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<IJobSettingStore>(_ => new RedisJobSettingStore(Connection, Prefix));
    }
}


