using System.Collections.Concurrent;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Jobs.Hangfire.Models;

namespace Krosoft.Extensions.Jobs.Hangfire.Storage.InMemory.Stores;

/// <summary>
/// Stockage en mémoire des définitions <see cref="IJobAutomatiqueSetting"/>.
/// </summary>
public class InMemoryJobSettingStore : IJobSettingStore
{
    private readonly ConcurrentDictionary<string, StoredJobSetting> _settings = new();

    public Task SaveAsync(IJobAutomatiqueSetting setting, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(setting.Identifiant))
        {
            throw new KrosoftTechnicalException("L'identifiant du job est requis.");
        }

        var now = DateTimeOffset.UtcNow;
        _settings.AddOrUpdate(setting.Identifiant!,
                              _ => new StoredJobSetting
                              {
                                  Identifiant = setting.Identifiant,
                                  CronExpression = setting.CronExpression,
                                  Type = setting.Type,
                                  QueueName = setting.QueueName,
                                  CreatedAt = now,
                                  UpdatedAt = now
                              },
                              (_, existing) => existing with
                              {
                                  Identifiant = setting.Identifiant,
                                  CronExpression = setting.CronExpression,
                                  Type = setting.Type,
                                  QueueName = setting.QueueName,
                                  UpdatedAt = now
                              });

        return Task.CompletedTask;
    }

    public Task<IEnumerable<StoredJobSetting>> GetAllAsync(CancellationToken cancellationToken)
        => Task.FromResult<IEnumerable<StoredJobSetting>>(_settings.Values.ToList());

    public Task<StoredJobSetting?> GetAsync(string identifiant, CancellationToken cancellationToken)
    {
        _settings.TryGetValue(identifiant, out var setting);
        return Task.FromResult(setting);
    }

    public Task RemoveAsync(string identifiant, CancellationToken cancellationToken)
    {
        _settings.TryRemove(identifiant, out _);
        return Task.CompletedTask;
    }
}
