using Hangfire;
using Krosoft.Extensions.Jobs.Hangfire.Interfaces;

namespace Krosoft.Extensions.Jobs.Hangfire.Models;

/// <summary>
/// Options de configuration pour le serveur Hangfire
/// </summary>
public sealed class KrosoftBackgroundJobServerOptions : BackgroundJobServerOptions
{
    /// <summary>
    /// Provider de stockage Hangfire
    /// </summary>
    public IHangfireStorageProvider? StorageProvider { get; set; }

    /// <summary>
    /// Configure un provider de stockage personnalisé
    /// </summary>
    public BackgroundJobServerOptions UseStorage(IHangfireStorageProvider storageProvider)
    {
        StorageProvider = storageProvider ?? throw new ArgumentNullException(nameof(storageProvider));
        return this;
    }
}