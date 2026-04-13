using Hangfire;
using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Jobs.Hangfire.Storage.InMemory.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Jobs.Hangfire.Storage.InMemory.Providers;

/// <summary>
/// Provider de stockage en mémoire pour Hangfire.
/// </summary>
public class InMemoryStorageProvider : IHangfireStorageProvider
{
    public void ConfigureStorage(IGlobalConfiguration configuration)
    {
        configuration.UseInMemoryStorage();
    }

    public void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<IJobSettingStore, InMemoryJobSettingStore>();
    }
}
