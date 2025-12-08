using Hangfire;

namespace Krosoft.Extensions.Jobs.Hangfire.Interfaces;

/// <summary>
/// Interface pour définir un provider de stockage Hangfire
/// </summary>
public interface IHangfireStorageProvider
{
    /// <summary>
    /// Configure le stockage Hangfire
    /// </summary>
    /// <param name="configuration">Configuration Hangfire</param>
    void ConfigureStorage(IGlobalConfiguration configuration);
}