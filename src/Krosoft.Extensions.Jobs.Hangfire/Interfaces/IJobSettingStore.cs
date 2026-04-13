using Krosoft.Extensions.Jobs.Hangfire.Models;

namespace Krosoft.Extensions.Jobs.Hangfire.Interfaces;

/// <summary>
/// Persistance des définitions <see cref="IJobAutomatiqueSetting"/> enrichies d'informations temporelles.
/// Alignée sur le backend Hangfire utilisé (en mémoire ou Redis).
/// </summary>
public interface IJobSettingStore
{
    Task SaveAsync(IJobAutomatiqueSetting setting, CancellationToken cancellationToken);
    Task<IEnumerable<StoredJobSetting>> GetAllAsync(CancellationToken cancellationToken);
    Task<StoredJobSetting?> GetAsync(string identifiant, CancellationToken cancellationToken);
    Task RemoveAsync(string identifiant, CancellationToken cancellationToken);
}
