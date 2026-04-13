using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Jobs.Hangfire.Interfaces;

/// <summary>
/// Interface pour définir un provider de stockage Hangfire.
/// </summary>
public interface IHangfireStorageProvider
{
    /// <summary>
    /// Configure le stockage Hangfire.
    /// </summary>
    /// <param name="configuration">Configuration Hangfire.</param>
    void ConfigureStorage(IGlobalConfiguration configuration);

    /// <summary>
    /// Enregistre les services complémentaires (ex: <see cref="IJobSettingStore"/>) alignés sur ce provider.
    /// </summary>
    /// <param name="services">Collection de services.</param>
    void RegisterServices(IServiceCollection services);
}
