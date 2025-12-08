using Hangfire;
using Krosoft.Extensions.Jobs.Hangfire.Interfaces;

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
}