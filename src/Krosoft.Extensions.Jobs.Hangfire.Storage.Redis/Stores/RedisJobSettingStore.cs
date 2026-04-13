using System.Text.Json;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using StackExchange.Redis;

namespace Krosoft.Extensions.Jobs.Hangfire.Storage.Redis.Stores;

/// <summary>
/// Stockage Redis des définitions <see cref="IJobAutomatiqueSetting" />.
/// </summary>
public class RedisJobSettingStore : IJobSettingStore
{
    private readonly IConnectionMultiplexer _connection;
    private readonly RedisKey _key;

    public RedisJobSettingStore(IConnectionMultiplexer connection, string prefix)
    {
        _connection = connection;
        _key = $"{prefix}job-settings";
    }

    public async Task SaveAsync(IJobAutomatiqueSetting setting, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(setting.Identifiant))
        {
            throw new KrosoftTechnicalException("L'identifiant du job est requis.");
        }

        var db = _connection.GetDatabase();
        var existing = await GetAsync(setting.Identifiant!, cancellationToken);

        var now = DateTimeOffset.UtcNow;
        var stored = new StoredJobSetting
        {
            Identifiant = setting.Identifiant,
            CronExpression = setting.CronExpression,
            Type = setting.Type,
            QueueName = setting.QueueName,
            CreatedAt = existing?.CreatedAt ?? now,
            UpdatedAt = now
        };

        await db.HashSetAsync(_key, setting.Identifiant, JsonSerializer.Serialize(stored));
    }

    public async Task<IEnumerable<StoredJobSetting>> GetAllAsync(CancellationToken cancellationToken)
    {
        var db = _connection.GetDatabase();
        var entries = await db.HashGetAllAsync(_key);

        var result = new List<StoredJobSetting>(entries.Length);
        foreach (var entry in entries)
        {
            if (entry.Value.HasValue)
            {
                var setting = JsonSerializer.Deserialize<StoredJobSetting>((string)entry.Value!);
                if (setting != null)
                {
                    result.Add(setting);
                }
            }
        }

        return result;
    }

    public async Task<StoredJobSetting?> GetAsync(string identifiant, CancellationToken cancellationToken)
    {
        var db = _connection.GetDatabase();
        var value = await db.HashGetAsync(_key, identifiant);
        if (!value.HasValue)
        {
            return null;
        }

        return JsonSerializer.Deserialize<StoredJobSetting>((string)value!);
    }

    public async Task RemoveAsync(string identifiant, CancellationToken cancellationToken)
    {
        var db = _connection.GetDatabase();
        await db.HashDeleteAsync(_key, identifiant);
    }
}